using UnityEngine;

namespace KitchenChaos.PlayerInput
{
    public class GamePauseMultiplayerUI : MonoBehaviour
    {
        Pause _pauseScript;

        private void Awake()
        {
            _pauseScript = FindObjectOfType<Pause>();
        }

        private void Start()
        {
            _pauseScript.OnMultiplayerPause += Pause_OnMultiplayerPause;

            DisplayPauseScreen(false);
        }

        void Pause_OnMultiplayerPause()
        {
            DisplayPauseScreen(!gameObject.activeSelf);
        }

        void DisplayPauseScreen(bool shouldDisplay)
        {
            gameObject.SetActive(shouldDisplay);
        }
    }
}
