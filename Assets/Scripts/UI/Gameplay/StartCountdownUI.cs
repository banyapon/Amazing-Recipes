using System;
using UnityEngine;
using TMPro;

namespace KitchenChaos.Core.UI
{
    public class StartCountdownUI : MonoBehaviour
    {
        public static event Action OnNumberChanged;

        [SerializeField] TextMeshProUGUI _countdownText;

        int _previousCountdownNumber;

        void Start()
        {
            GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
            DisplayCountdown(false);
        }

        void Update()
        {
            if (!GameManager.Instance.IsCountdownActive()) return;

            int countdownNumber = Mathf.CeilToInt(GameManager.Instance.CountdownToStartTimer);
            _countdownText.text = countdownNumber.ToString();

            if (_previousCountdownNumber != countdownNumber)
            {
                _previousCountdownNumber = countdownNumber;          
                OnNumberChanged?.Invoke();
            }
        }

        void GameManager_OnStateChanged()
        {
            DisplayCountdown(GameManager.Instance.IsCountdownActive());
        }

        void DisplayCountdown(bool shouldDisplay)
        {
            _countdownText.gameObject.SetActive(shouldDisplay);
        }

        void OnDestroy()
        {
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        }

        public static void ResetStaticData()
        {
            OnNumberChanged = null;
        }
    }
}
