using UnityEngine;
using KitchenChaos.Core;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace KitchenChaos.PlayerInput
{
    public class Pause : NetworkBehaviour
    {
        public event Action OnPauseButtonPressed;
        public event Action OnMultiplayerPause;

        [SerializeField] GameInput _input;
        bool _isLocalGamePaused;
        public bool Paused => _isLocalGamePaused;
        NetworkVariable<bool> _isGamePaused = new NetworkVariable<bool>(false);

        Dictionary<ulong, bool> _playerPausedDictionary;

        private void Awake()
        {
            _playerPausedDictionary = new Dictionary<ulong, bool>();
        }

        void Start()
        {
            _input.OnPauseAction += GameInput_OnPauseAction;
        }

        public override void OnNetworkSpawn()
        {
            _isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

            if (IsServer)
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }

        // test 
        void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (clientId == OwnerClientId && _isLocalGamePaused)
            {
                _isLocalGamePaused = false;
                CheckForAnyPlayerPaused();
            }
        }

        private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
        {
            if (_isGamePaused.Value)
            {
                Time.timeScale = 0f;
                OnMultiplayerPause?.Invoke();
            }
            else
            {
                OnMultiplayerPause?.Invoke();
                Time.timeScale = 1f;
            }
        }

        void GameInput_OnPauseAction()
        {
            TogglePauseGame();
        }

        internal void TogglePauseGame()
        {
            if (GameManager.Instance.IsGameOver()) return;
        
            _isLocalGamePaused = !_isLocalGamePaused;


            if (_isLocalGamePaused)
                PauseGameServerRpc();
            else
                UnpauseGameServerRpc();

            OnPauseButtonPressed?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            _playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

            CheckForAnyPlayerPaused();
        }

        [ServerRpc(RequireOwnership = false)]
        void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            _playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

            CheckForAnyPlayerPaused();
        }

        void CheckForAnyPlayerPaused()
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (_playerPausedDictionary.ContainsKey(clientId) && _playerPausedDictionary[clientId])
                {
                    // this player is paused
                    _isGamePaused.Value = true;
                    return;
                }
            }

            // all players are unpaused
            _isGamePaused.Value = false;
        }

        //on despawn instead??
        public override void OnDestroy()
        {
            _input.OnPauseAction -= GameInput_OnPauseAction;
        }
    }
}
