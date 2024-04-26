using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KitchenChaos.Interactions.Multiplayer;

namespace KitchenChaos.Multiplayer.UI
{
    public class ConnectingUI : MonoBehaviour
    {
        private void Start()
        {
            GameMultiplayer.Instance.OnTryingToJoinGame += GameMultiplayer_OnTryingToJoinGame;
            GameMultiplayer.Instance.OnFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;

            Hide();
        }

        void GameMultiplayer_OnTryingToJoinGame()
        {
            Show();
        }

        void GameMultiplayer_OnFailedToJoinGame()
        {
            Hide();
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
            GameMultiplayer.Instance.OnTryingToJoinGame -= GameMultiplayer_OnTryingToJoinGame;
            GameMultiplayer.Instance.OnFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;
        }
    }
}
