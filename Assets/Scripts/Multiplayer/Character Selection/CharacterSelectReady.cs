using System;
using System.Collections.Generic;
using Unity.Netcode;
using KitchenChaos.Core;

namespace KitchenChaos.Multiplayer
{
    public class CharacterSelectReady : NetworkBehaviour
    {
        public event Action OnReadyChanged;

        Dictionary<ulong, bool> _playerReadyDictionary;

        void Awake()
        {
            _playerReadyDictionary = new Dictionary<ulong, bool>();
        }

        public void SetPlayerReady()
        {
            print("Player ready!");
            SetPlayerReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

            _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            bool allClientsReady = true;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!_playerReadyDictionary.ContainsKey(clientId) || !_playerReadyDictionary[clientId])
                {
                    allClientsReady = false;
                    break;
                }
            }

            if (allClientsReady)
            {
                KitchenGameLobby.Instance.DeleteLobby();
                Loader.LoadSceneNetwork(Loader.Scene.GameScene);
            }
        }

        [ClientRpc]
        void SetPlayerReadyClientRpc(ulong clientId)
        {
            _playerReadyDictionary[clientId] = true;
            OnReadyChanged?.Invoke();
        }

        public bool IsPlayerReady(ulong clientId)
        {
            return _playerReadyDictionary.ContainsKey(clientId) && _playerReadyDictionary[clientId];
        }

    }
}
