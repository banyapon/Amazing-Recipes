using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Multiplayer.UI
{
    public class LobbyCreateUI : MonoBehaviour
    {
        [SerializeField] Button _closeButton;
        [SerializeField] Button _createPublicButton;
        [SerializeField] Button _createPrivateButton;
        [SerializeField] Button _preselectedButton;
        [SerializeField] TMP_InputField _lobbyNameInputField;

        void Awake()
        {
            _createPublicButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.CreateLobby(_lobbyNameInputField.text, false);
            });

            _createPrivateButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.CreateLobby(_lobbyNameInputField.text, true);
            });

            _closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        void Start()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _preselectedButton.Select();
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}
