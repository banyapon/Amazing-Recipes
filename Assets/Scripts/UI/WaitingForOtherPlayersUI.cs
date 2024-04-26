using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KitchenChaos.Core;

namespace KitchenChaos.PlayerInput.UI
{
    public class WaitingForOtherPlayersUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
            GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

            Hide();
        }

        private void GameManager_OnStateChanged()
        {
            if (GameManager.Instance.IsCountdownActive())
                Hide();
        }

        private void GameManager_OnLocalPlayerReadyChanged()
        {
            if (GameManager.Instance.IsLocalPlayerReady)
                Show();
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}
