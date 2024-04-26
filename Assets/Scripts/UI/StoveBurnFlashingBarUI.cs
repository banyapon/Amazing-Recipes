using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions.UI.Animation
{
    public class StoveBurnFlashingBarUI : MonoBehaviour
    {
        [SerializeField] StoveCounter _stoveCounter;
        Animator _animator;
        const string IS_FLASHING = "IsFlashing";

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            _animator.SetBool(IS_FLASHING, false);
        }

        void StoveCounter_OnProgressChanged(float progress)
        {
            float burnShowProgressAmount = .5f;
            bool show = _stoveCounter.IsFried() && progress >= burnShowProgressAmount;

            _animator.SetBool(IS_FLASHING, show);
        }
    }
}
