using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using KitchenChaos.Core;
using KitchenChaos.Interactions;
using Unity.Services.Authentication;

namespace KitchenChaos.Multiplayer
{
    public class GameMultiplayer : NetworkBehaviour
    {
        public static GameMultiplayer Instance { get; private set; }

        public static bool PlayMultiplayer;

        public event Action OnTryingToJoinGame;
        public event Action OnFailedToJoinGame;
        public event Action OnPlayerDataNetworkListChanged;

        public const int MAX_NUMBER_PLAYERS = 4;
        public const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";
        
        [SerializeField] SO_KitchenObjectList _kitchenObjectListSO;
        [SerializeField] List<Color> _playerColorList;

        NetworkList<PlayerData> _playerDataNetworkList;
        string _playerName;

        void Awake()
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            string defaultName = "Player" + UnityEngine.Random.Range(2, 1000).ToString();
            _playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, defaultName);

            _playerDataNetworkList = new NetworkList<PlayerData>();
            _playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
        }

        void Start()
        {
            if (!PlayMultiplayer)
            {
                StartHost();
                Loader.LoadSceneNetwork(Loader.Scene.GameScene);
            }
        }

        private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
        {
            OnPlayerDataNetworkListChanged?.Invoke();
        }

        public void StartHost()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;

            NetworkManager.Singleton.StartHost();
        }

        private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
        {
            for (int i = 0; i < _playerDataNetworkList.Count; i++)
            {
                PlayerData playerData = _playerDataNetworkList[i];
                if (playerData.ClientId == clientId)
                {
                    // disconnected
                    _playerDataNetworkList.RemoveAt(i);
                }
            }
        }

        void NetworkManager_OnClientConnectedCallback(ulong clientId)
        {
            _playerDataNetworkList.Add(new PlayerData
            {
                ClientId = clientId,
                ColorId = GetFirstUnusedColorId(),
            });

            SetPlayerNameServerRpc(_playerName);
            SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
        }

        void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
            NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {

            if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
            {
                connectionApprovalResponse.Approved = false;
                connectionApprovalResponse.Reason = "Game has already started";
                return;
            }

            if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_NUMBER_PLAYERS)
            {
                connectionApprovalResponse.Approved = false;
                connectionApprovalResponse.Reason = "Game is full";
                return;
            }

            connectionApprovalResponse.Approved = true;
        }

        public void StartClient()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
            OnTryingToJoinGame?.Invoke();
            NetworkManager.Singleton.StartClient();
        }

        void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
        {
            SetPlayerNameServerRpc(_playerName);
            SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
        }

        [ServerRpc(RequireOwnership = false)]
        void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
            PlayerData playerData = _playerDataNetworkList[playerDataIndex];

            playerData.PlayerName = playerName;

            _playerDataNetworkList[playerDataIndex] = playerData;

        }

        [ServerRpc(RequireOwnership = false)]
        void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
            PlayerData playerData = _playerDataNetworkList[playerDataIndex];

            playerData.PlayerId = playerId;

            _playerDataNetworkList[playerDataIndex] = playerData;

        }

        void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
        {
            OnFailedToJoinGame?.Invoke();
        }

        //for prototyping only - implement pooling
        public void SpawnKitchenObject(SO_KitchenObject kitchenObjectSO, IKitchenObjectHolder kitchenObjectHolder)
        {
            int kitchenObjectSOIndex = GetKitchenObjectSOIndex(kitchenObjectSO);

            SpawnKitchenObjectServerRpc(kitchenObjectSOIndex, kitchenObjectHolder.GetNetworkObject());
        }

        // Only server can spawn networkObjects so serverRpc is needed,
        // but we want client to be able to call this function.
        // Turned SO_KitchenObject to int and IKitchenObjectHolder to NetworkObjectReference
        // for serialization purposes.
        [ServerRpc(RequireOwnership = false)]
        void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectHolder_NetworkObjRef)
        {
            SO_KitchenObject kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

            kitchenObjectHolder_NetworkObjRef.TryGet(out NetworkObject kitchenObjectHolder_NetworkObject);
            IKitchenObjectHolder kitchenObjectHolder = kitchenObjectHolder_NetworkObject.GetComponent<IKitchenObjectHolder>();

            if (kitchenObjectHolder.HasKitchenObject()) return;

            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.KitchenObjectPrefab);

            // Spawn object on network
            NetworkObject kitchenObject_NetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
            kitchenObject_NetworkObject.Spawn(true);

            // Set KitchenObject holder/parent
            KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

            kitchenObject.SetKitchenObjectHolder(kitchenObjectHolder);
        }

        public int GetKitchenObjectSOIndex(SO_KitchenObject kitchenObjectSO)
        {
            return _kitchenObjectListSO.KitchenObjectList.IndexOf(kitchenObjectSO);
        }

        public SO_KitchenObject GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
        {
            return _kitchenObjectListSO.KitchenObjectList[kitchenObjectSOIndex];
        }

        public void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObject_NetworkObjRef)
        {
            kitchenObject_NetworkObjRef.TryGet(out NetworkObject kitchenObject_NetworkObject);

            if (kitchenObject_NetworkObject == null) return;

            KitchenObject kitchenObject = kitchenObject_NetworkObject.GetComponent<KitchenObject>();

            ClearKitchenObjectHolderClientRpc(kitchenObject_NetworkObjRef);
            //only server can destroy objects
            kitchenObject.DestroySelf();
        }

        [ClientRpc]
        void ClearKitchenObjectHolderClientRpc(NetworkObjectReference kitchenObject_NetworkObjRef)
        {
            kitchenObject_NetworkObjRef.TryGet(out NetworkObject kitchenObject_NetworkObject);
            KitchenObject kitchenObject = kitchenObject_NetworkObject.GetComponent<KitchenObject>();

            kitchenObject.ClearKitchenObjectHolder();
        }

        public bool IsPlayerIndexConnected(int playerIndex)
        {
            return playerIndex < _playerDataNetworkList.Count;
        }

        public PlayerData GetPlayerDataFromClientId(ulong clientId)
        {
            foreach (var playerData in _playerDataNetworkList)
            {
                if (playerData.ClientId == clientId)
                    return playerData;
            }

            return default;
        }

        public PlayerData GetPlayerData()
        {
            return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
        }

        public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
        {
            return _playerDataNetworkList[playerIndex];
        }

        public int GetPlayerDataIndexFromClientId(ulong clientId)
        {
            for (int i = 0; i < _playerDataNetworkList.Count; i++)
            {
                if (_playerDataNetworkList[i].ClientId == clientId)
                    return i;
            }

            return -1;
        }

        public Color GetPlayerColor(int colorId)
        {
            return _playerColorList[colorId];
        }

        public void ChangePlayerColor(int colorId)
        {
            ChangePlayerColorServerRpc(colorId);
        }

        [ServerRpc(RequireOwnership = false)]
        void ChangePlayerColorServerRpc(int colorId, ServerRpcParams rpcParams = default)
        {
            if (!IsColorAvailable(colorId))
                return;

            int playerDataIndex = GetPlayerDataIndexFromClientId(rpcParams.Receive.SenderClientId);

            PlayerData playerData = _playerDataNetworkList[playerDataIndex];

            playerData.ColorId = colorId;

            _playerDataNetworkList[playerDataIndex] = playerData;

        }

        bool IsColorAvailable(int colorId)
        {
            foreach (var playerData in _playerDataNetworkList)
            {
                if (playerData.ColorId == colorId)
                {
                    //already in use
                    return false;
                }
            }

            return true;
        }

        int GetFirstUnusedColorId()
        {
            for (int i = 0; i < _playerColorList.Count; i++)
            {
                if (IsColorAvailable(i))
                    return i;
            }

            return -1;
        }

        public void KickPlayer(ulong clientId)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            NetworkManager_Server_OnClientDisconnectCallback(clientId);
        }


        public string GetPlayerName()
        {
            return _playerName;
        }

        public void SetPlayerName(string name)
        {
            _playerName = name;

            PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, name);
        }

    }
}
