using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using KitchenChaos.Interactions.Multiplayer;

namespace KitchenChaos.Multiplayer.UI
{
    public class TestingNetcodeUI : MonoBehaviour
    {
        [SerializeField] Button _startHostButton;
        [SerializeField] Button _startClientButton;

        void Awake()
        {
            _startHostButton.onClick.AddListener(() =>
            {
                Debug.Log("Host");
                GameMultiplayer.Instance.StartHost();
                Hide();
            });

            _startClientButton.onClick.AddListener(() =>
            {
                Debug.Log("Client");
                GameMultiplayer.Instance.StartClient();
                Hide();
            });
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
