using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using KitchenChaos.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

namespace KitchenChaos.Multiplayer
{
    public class KitchenGameLobby : MonoBehaviour
    {
        public static KitchenGameLobby Instance { get; private set; }

        public event Action OnCreateLobbyStarted;
        public event Action OnCreateLobbyFailed;
        public event Action OnJoinStarted;
        public event Action OnJoinFailed;
        public event Action OnQuickJoinFailed;
        public event Action<List<Lobby>> OnLobbyListChanged;

        [SerializeField] float _heartBeatTimerMax = 15f;
        [SerializeField] float _refreshLobbiesTimerMax = 3f;

        const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";

        Lobby _joinedLobby;
        float _heartBeatTimer;
        float _refreshLobbiesTimer;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAuthentication();
        }

        void Update()
        {
            // error? -> move check to RefreshLobbyList
            if (SceneManager.GetActiveScene().name != Loader.Scene.LobbyScene.ToString())
                return;

            HandleHeartbeat();
            PeriodicallyRefreshLobbyList();
        }

        void HandleHeartbeat()
        {
            if (!IsLobbyHost()) return;

            _heartBeatTimer -= Time.deltaTime;

            if (_heartBeatTimer <= 0)
            {
                _heartBeatTimer = _heartBeatTimerMax;
                LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
            }
        }

        void PeriodicallyRefreshLobbyList()
        {
            // if this throws error, wrap in if with &&
            if (_joinedLobby != null || !AuthenticationService.Instance.IsSignedIn) 
                return;

            _refreshLobbiesTimer -= Time.deltaTime;

            if (_refreshLobbiesTimer <= 0)
            {
                _refreshLobbiesTimer = _refreshLobbiesTimerMax;
                ListLobbies();
            }
        }

        bool IsLobbyHost()
        {
            return _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

        async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                    }
                };

                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                OnLobbyListChanged?.Invoke(queryResponse.Results);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        async void InitializeAuthentication()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized) return;

            // init with a different profile every time (for testing on the same unit)
            InitializationOptions initializationOptions = new InitializationOptions();
            //initializationOptions.SetProfile("Player" + UnityEngine.Random.Range(1, 100).ToString());

            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        async Task<Allocation> AllocateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(GameMultiplayer.MAX_NUMBER_PLAYERS - 1);
                return allocation;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
        }

        async Task SetRelay()
        {
            Allocation allocation = await AllocateRelay();
            string relayJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                }
            });

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        }

        async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return relayCode;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
        }

        async Task<JoinAllocation> JoinRelayWCode(string code)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);
                return joinAllocation;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
        }

        async Task JoinRelay()
        {
            string relayJoinCode = _joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelayWCode(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        }

        public async void CreateLobby(string lobbyName, bool isPrivate)
        {
            OnCreateLobbyStarted?.Invoke();

            try
            {
                _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, GameMultiplayer.MAX_NUMBER_PLAYERS, new CreateLobbyOptions
                {
                    IsPrivate = isPrivate
                });

                await SetRelay();

                GameMultiplayer.Instance.StartHost();
                Loader.LoadSceneNetwork(Loader.Scene.CharacterSelectScene);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnCreateLobbyFailed?.Invoke();
            }
        }

        public async void QuickJoin()
        {
            OnJoinStarted?.Invoke();

            try
            {
                _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

                await JoinRelay();

                GameMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnQuickJoinFailed?.Invoke();
            }
        }

        public async void JoinWithCode(string lobbyCode)
        {
            OnJoinStarted?.Invoke();

            try
            {
                _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

                await JoinRelay();

                GameMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnJoinFailed?.Invoke();
            }
        }

        //public lobbies cannot be joined by code!
        public async void JoinWithId(string lobbyId)
        {
            OnJoinStarted?.Invoke();

            try
            {
                _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

                await JoinRelay();

                GameMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnJoinFailed?.Invoke();
            }
        }

        public Lobby GetLobby()
        {
            return _joinedLobby;
        }

        public async void DeleteLobby()
        {
            if (_joinedLobby == null) return;

            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

        }

        public async void LeaveLobby()
        {
            if (_joinedLobby == null) return;

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void KickPlayer(string playerId)
        {
            if (!IsLobbyHost()) return;

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

    }
}
