using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using KitchenChaos.PlayerInput;

namespace KitchenChaos.Core
{
    public class GameManager : NetworkBehaviour
    {
        //use service locator, there are quite a few singletons already
        public static GameManager Instance { get; private set; }
        public event Action OnStateChanged;
        public event Action OnLocalPlayerReadyChanged;

        [SerializeField] Transform _playerPrefab;
        [SerializeField] GameInput _gameInput;
        [SerializeField] NetworkVariable<float> _countdownToStartTimer = new NetworkVariable<float>(3f);
        public float CountdownToStartTimer => _countdownToStartTimer.Value;
        private Transform player;

        [SerializeField] float _playingTimerMax = 10f;

        NetworkVariable<float> _playingTimer = new NetworkVariable<float>(0f);
        bool _isLocalPlayerReady;
        public bool IsLocalPlayerReady => _isLocalPlayerReady;

        Dictionary<ulong, bool> _playerReadyDictionary;

        enum GameplayState
        {
            WaitingToStart,
            CountdownToStart,
            GamePlayingTime,
            GameOver,
        }

        NetworkVariable<GameplayState> _state= new NetworkVariable<GameplayState>(GameplayState.WaitingToStart);
        Dictionary<GameplayState, Action> _updateMethods = new Dictionary<GameplayState, Action>();

        void Awake()
        {
            Instance = this;
            FillStateDictionary();

            _playerReadyDictionary = new Dictionary<ulong, bool>();
        }

        void Start()
        {
            _gameInput.OnInteractAction += GameInput_OnInteractAction;
        }

        public override void OnNetworkSpawn()
        {
            _state.OnValueChanged += GameplayState_OnValueChanged;

            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

        void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                player = Instantiate(_playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
                player.gameObject.name = ""+ clientId;
            }
        }

        void GameplayState_OnValueChanged(GameplayState previousValue, GameplayState newValue)
        {
            OnStateChanged?.Invoke();
        }

        void GameInput_OnInteractAction()
        {
            if (_state.Value == GameplayState.WaitingToStart)
            {
                _isLocalPlayerReady = true;
                OnLocalPlayerReadyChanged?.Invoke();
                
                SetPlayerReadyServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            bool allClientsReady = true;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                // all players are not ready if the dict doesn't contain a clientId or
                // it does, but the value is false
                if (!_playerReadyDictionary.ContainsKey(clientId) || !_playerReadyDictionary[clientId])
                {
                    allClientsReady = false;
                    break;
                }
            }

            if (allClientsReady)
                _state.Value = GameplayState.CountdownToStart;
        }

        void Update()
        {
            if (!IsServer) return;

            _updateMethods[_state.Value].Invoke();

            
        }

        void FillStateDictionary()
        {
            _updateMethods[GameplayState.WaitingToStart] = UpdateWaitingToStart;
            _updateMethods[GameplayState.CountdownToStart] = UpdateCountdownToStart;
            _updateMethods[GameplayState.GamePlayingTime] = UpdateGamePlayingTime;
            _updateMethods[GameplayState.GameOver] = UpdateGameOver;
        }

        void UpdateWaitingToStart()
        {
        }

        void UpdateCountdownToStart()
        {
            _countdownToStartTimer.Value -= Time.deltaTime;
            if (_countdownToStartTimer.Value < 0f)
            {
                _state.Value = GameplayState.GamePlayingTime;
                _playingTimer.Value = _playingTimerMax;
            }
        }

        void UpdateGamePlayingTime()
        {
            _playingTimer.Value -= Time.deltaTime;
            if (_playingTimer.Value < 0f)
            {
                _state.Value = GameplayState.GameOver;
            }
        }

        void UpdateGameOver()
        {
        }

        public bool IsWaitingToStart()
        {
            return _state.Value == GameplayState.WaitingToStart;
        }

        public bool IsGamePlaying()
        {
            return _state.Value == GameplayState.GamePlayingTime;
        }

        public bool IsCountdownActive()
        {
            return _state.Value == GameplayState.CountdownToStart;
        }

        public bool IsGameOver()
        {
            return _state.Value == GameplayState.GameOver;
        }

        public float GetGamePlayingTimeNormalized()
        {
            return 1 - (_playingTimer.Value / _playingTimerMax);
        }
    }
}
