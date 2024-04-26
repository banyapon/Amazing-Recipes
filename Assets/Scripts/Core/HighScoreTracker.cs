using UnityEngine;

namespace KitchenChaos.Core
{
    public class HighScoreTracker : MonoBehaviour
    {
        const string HIGHSCORE_KEY = "Highscore";

        void Start()
        {
            DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;
        }

        void DeliveryManager_OnRecipeDelivered()
        {
            int score = DeliveryManager.Instance.DeliveredRecipesScore;

            if (score > GetCurrentHighScore())
                SaveHighScore(score);

            Debug.Log($"Score: {score}, high score: {GetCurrentHighScore()}");
        }

        void SaveHighScore(float score)
        {
            PlayerPrefs.SetFloat(HIGHSCORE_KEY, score);
        }

        public int GetCurrentHighScore()
        {
            if (PlayerPrefs.HasKey(HIGHSCORE_KEY))
                return Mathf.RoundToInt(PlayerPrefs.GetFloat(HIGHSCORE_KEY));

            else return 0;
        }
    }
}
