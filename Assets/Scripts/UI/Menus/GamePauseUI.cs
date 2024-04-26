using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KitchenChaos.Core;
using Unity.Netcode;

namespace KitchenChaos.PlayerInput.UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] Pause _pauseScript;
        [SerializeField] Button _menuButton;
        [SerializeField] Button _resumeButton;

        [SerializeField] Button _preselectedButton;

        void Awake()
        {
            SetupButtonListeners();
        }

        void Start()
        {
            _pauseScript.OnPauseButtonPressed += _pauseScript_OnPauseButtonPressed;
            DisplayPauseScreen(false);
        }

        void _pauseScript_OnPauseButtonPressed()
        {
            DisplayPauseScreen(_pauseScript.Paused);

            if (_pauseScript.Paused)
                _preselectedButton.Select();
        }

        void DisplayPauseScreen(bool shouldDisplay)
        {
            gameObject.SetActive(shouldDisplay);
        }

        void SetupButtonListeners()
        {
            _menuButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                Loader.LoadScene(Loader.Scene.MainMenuScene);
            });

            _resumeButton.onClick.AddListener(() =>
            {
                _pauseScript.TogglePauseGame();
            });
        }

        void OnDestroy()
        {
            _pauseScript.OnPauseButtonPressed -= _pauseScript_OnPauseButtonPressed;
        }
    }
}
