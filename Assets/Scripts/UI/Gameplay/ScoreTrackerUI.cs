using UnityEngine;
using TMPro;

namespace KitchenChaos.Core.UI
{
    public class ScoreTrackerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;

        private void Start()
        {
            _scoreText.text = "0";

            DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;
        }

        private void DeliveryManager_OnRecipeDelivered()
        {
            _scoreText.text = DeliveryManager.Instance.DeliveredRecipesScore.ToString();
        }
    }
}
