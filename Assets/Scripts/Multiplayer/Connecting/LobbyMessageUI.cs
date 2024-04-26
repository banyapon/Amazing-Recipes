using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

namespace KitchenChaos.Multiplayer.UI
{
    public class LobbyMessageUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _messageText;
        [SerializeField] Button _closeButton;

        void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
        }

        void Start()
        {
            GameMultiplayer.Instance.OnFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;

            KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
            KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
            KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
            KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
            KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

            Hide();
        }

        void KitchenGameLobby_OnQuickJoinFailed()
        {
            ShowMessage("Could not find a lobby to quick join!");
            DisplayButton(true);
        }

        void KitchenGameLobby_OnJoinFailed()
        {
            ShowMessage("Failed to join lobby!");
            DisplayButton(true);
        }

        void KitchenGameLobby_OnJoinStarted()
        {
            ShowMessage("Joining lobby...");
            DisplayButton(false);
        }

        void KitchenGameLobby_OnCreateLobbyFailed()
        {
            ShowMessage("Failed to create lobby!");
            DisplayButton(true);
        }

        void KitchenGameLobby_OnCreateLobbyStarted()
        {
            ShowMessage("Creating lobby...");
            DisplayButton(false);
        }

        void GameMultiplayer_OnFailedToJoinGame()
        {
            if (NetworkManager.Singleton.DisconnectReason == "")
                ShowMessage("Failed to connect!");
            else
                ShowMessage(NetworkManager.Singleton.DisconnectReason);

            DisplayButton(true);
        }

        void ShowMessage(string message)
        {
            Show();

            _messageText.text = message;
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        void DisplayButton(bool shouldDisplay)
        {
            _closeButton.gameObject.SetActive(shouldDisplay);
        }

        void OnDestroy()
        {
            GameMultiplayer.Instance.OnFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;

            KitchenGameLobby.Instance.OnCreateLobbyStarted -= KitchenGameLobby_OnCreateLobbyStarted;
            KitchenGameLobby.Instance.OnCreateLobbyFailed -= KitchenGameLobby_OnCreateLobbyFailed;
            KitchenGameLobby.Instance.OnJoinStarted -= KitchenGameLobby_OnJoinStarted;
            KitchenGameLobby.Instance.OnJoinFailed -= KitchenGameLobby_OnJoinFailed;
            KitchenGameLobby.Instance.OnQuickJoinFailed -= KitchenGameLobby_OnQuickJoinFailed;
        }
    }
}
