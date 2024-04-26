using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using KitchenChaos.Core;
using TMPro;
using Unity.Services.Lobbies.Models;

namespace KitchenChaos.Multiplayer.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] Button _mainMenuButton;
        [SerializeField] Button _readyButton;
        [SerializeField] CharacterSelectReady _characterSelectReady;
        [SerializeField] TextMeshProUGUI _lobbyNameText;
        [SerializeField] TextMeshProUGUI _lobbyCodeText;

        void Awake()
        {
            _mainMenuButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.LeaveLobby();
                NetworkManager.Singleton.Shutdown();
                Loader.LoadScene(Loader.Scene.MainMenuScene);
            });

            _readyButton.onClick.AddListener(() =>
            {
                _characterSelectReady.SetPlayerReady();
            });
        }

        void Start()
        {
            Lobby lobby = KitchenGameLobby.Instance.GetLobby();

            _lobbyNameText.text = "Lobby: " + lobby.Name;
            _lobbyCodeText.text = lobby.LobbyCode;
        }



    }
}
