using KitchenChaos.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Core
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] Button _playMultiplayerButton;
        [SerializeField] Button _playSingleplayerButton;
        [SerializeField] Button _quitButton;

        void Awake()
        {
            Time.timeScale = 1f;
            SetupButtonListeners();
        }

        void Start()
        {
            _playMultiplayerButton.Select();
        }

        void SetupButtonListeners()
        {
            _playMultiplayerButton.onClick.AddListener(() =>
            {
                GameMultiplayer.PlayMultiplayer = true;
                Loader.LoadScene(Loader.Scene.LobbyScene);
            });

            //why go through Lobby? Work around?
            _playSingleplayerButton.onClick.AddListener(() =>
            {
                GameMultiplayer.PlayMultiplayer = false;
                Loader.LoadScene(Loader.Scene.LobbyScene);
            });

            _quitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}
