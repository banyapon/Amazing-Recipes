using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Core.UI
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField] Image _timerImage;

        private void Update()
        {
            _timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimeNormalized();
        }
    }
}
