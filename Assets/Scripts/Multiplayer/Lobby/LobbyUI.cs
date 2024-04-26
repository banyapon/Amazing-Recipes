using KitchenChaos.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Multiplayer.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] Button _mainMenuButton;
        [SerializeField] Button _createLobbyButton;
        [SerializeField] Button _quickJoinButton;
        [SerializeField] Button _joinWithCodeButton;
        [Space]
        [SerializeField] TMP_InputField _joinCodeInputField;
        [SerializeField] TMP_InputField _playerNameInputField;
        [Space]
        [SerializeField] LobbyCreateUI _lobbyCreateUI;
        [Space]
        [SerializeField] Transform _lobbyContainer;
        [SerializeField] Transform _lobbyTemplate;

        void Awake()
        {
            _mainMenuButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.LeaveLobby();
                Loader.LoadScene(Loader.Scene.MainMenuScene);
            });

            _createLobbyButton.onClick.AddListener(() =>
            {
                _lobbyCreateUI.Show();
            });

            _quickJoinButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.QuickJoin();
            });

            _joinWithCodeButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.JoinWithCode(_joinCodeInputField.text);
            });

            //_lobbyTemplate.gameObject.SetActive(false);
        }

        void Start()
        {
            _playerNameInputField.text = GameMultiplayer.Instance.GetPlayerName();

            _playerNameInputField.onValueChanged.AddListener((string newName) =>
            {
                GameMultiplayer.Instance.SetPlayerName(newName);
            });

            KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
            UpdateLobbyList(new List<Lobby>());
        }

        void KitchenGameLobby_OnLobbyListChanged(List<Lobby> lobbyList)
        {
            UpdateLobbyList(lobbyList);
        }

        void UpdateLobbyList(List<Lobby> lobbyList)
        {
            // consider not destroying every time
            // instead just change the contents?
            foreach (Transform child in _lobbyContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbyList)
            {
                Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);
                lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
            }
        }

        void OnDestroy()
        {
            KitchenGameLobby.Instance.OnLobbyListChanged -= KitchenGameLobby_OnLobbyListChanged;
        }
    }
}
