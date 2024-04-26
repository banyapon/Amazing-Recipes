using UnityEngine;

namespace KitchenChaos.Core.UI.Animation
{
    public class StartCountdownUI_Animation : MonoBehaviour
    {
        const string NUMBER_POPUP = "NumberPopUp";
        Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            StartCountdownUI.OnNumberChanged += StartCountdownUI_OnNumberChanged;
        }

        void StartCountdownUI_OnNumberChanged()
        {
            _animator.SetTrigger(NUMBER_POPUP);
        }

        void OnDestroy()
        {
            StartCountdownUI.OnNumberChanged -= StartCountdownUI_OnNumberChanged;
        }
    }
}
