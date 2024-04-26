using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Core.UI
{
    public class HostDisconnectedUI : MonoBehaviour
    {
        [SerializeField] Button _restartButton;

        void Awake()
        {
            _restartButton.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.Scene.MainMenuScene);
            });
        }

        void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

            Hide();
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                // server disconnected
                Show();
            }
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }
}
