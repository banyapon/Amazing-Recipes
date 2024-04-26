using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KitchenChaos.Core.UI
{
    public class DeliveryResultUI : MonoBehaviour
    {
        [SerializeField] Image _backgroundImage;
        [SerializeField] Image _icon;
        [SerializeField] TextMeshProUGUI _message;
        [SerializeField] Color _successColor;
        [SerializeField] Color _failColor;
        [SerializeField] Sprite _successSprite;
        [SerializeField] Sprite _failSprite;

        Animator _animator;
        const string POPUP = "PopUp";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

            gameObject.SetActive(false);
        }

        void DeliveryManager_OnRecipeFailed()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger(POPUP);
            _backgroundImage.color = _failColor;
            _icon.sprite = _failSprite;
            _message.text = "DELIVERY\nFAIL";
        }

        void DeliveryManager_OnRecipeDelivered()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger(POPUP);
            _backgroundImage.color = _successColor;
            _icon.sprite = _successSprite;
            _message.text = "DELIVERY\nSUCCESS";
        }

        public void DisplayResult(bool display)
        {
            gameObject.SetActive(display);
        }
    }
}
