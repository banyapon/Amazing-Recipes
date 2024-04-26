using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

namespace KitchenChaos.Core.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _recipesDeliveredText;
        [SerializeField] TextMeshProUGUI _deliveredScoreText;
        [SerializeField] TextMeshProUGUI _highScoreText;

        [SerializeField] Button _restartButton;
        [SerializeField] Button _menuButton;
        [SerializeField] Button _preselectedButton;
        
        [SerializeField] HighScoreTracker _highScoreTracker;

        void Start()
        {
            GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

            _restartButton.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.Scene.GameScene);
            });

            _menuButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                Loader.LoadScene(Loader.Scene.MainMenuScene);
            });

            DisplayGameOverUI(false);
        }

        //show the recipes text as numbers counting up from 0 - see how to do this
        void GameManager_OnStateChanged()
        {
            DisplayGameOverUI(GameManager.Instance.IsGameOver());

            if (!gameObject.activeInHierarchy) return;
            
            _recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulRecipesDelivered.ToString();
            _deliveredScoreText.text = DeliveryManager.Instance.DeliveredRecipesScore.ToString();
            _highScoreText.text = _highScoreTracker.GetCurrentHighScore().ToString();
        }

        void DisplayGameOverUI(bool shouldDisplay)
        {
            gameObject.SetActive(shouldDisplay);

            if (shouldDisplay)
                _preselectedButton.Select();
        }

        IEnumerator CO_DisplayDeliveredRecipesNumber()
        {
            int count = 0;
            //int recipesDelivered = DeliveryManager.Instance.SuccessfulRecipesDelivered;
            int recipesDelivered = 10;

            while (count <= recipesDelivered)
            {
                _recipesDeliveredText.text = count.ToString();
                count++;
                yield return new WaitForEndOfFrame();
            }
        }

        void OnDestroy()
        {
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        }
    }
}
