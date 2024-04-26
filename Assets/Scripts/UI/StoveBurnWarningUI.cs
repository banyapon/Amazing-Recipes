using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions.UI
{
    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] StoveCounter _stoveCounter;

        void Start()
        {
            _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            DisplayWarningSign(false);
        }

        void StoveCounter_OnProgressChanged(float progress)
        {
            float burnShowProgressAmount = .5f;
            bool show = _stoveCounter.IsFried() && progress >= burnShowProgressAmount;

            if (show)
                DisplayWarningSign(true);
            else
                DisplayWarningSign(false);      
        }

        void DisplayWarningSign(bool shouldDisplay)
        {
            gameObject.SetActive(shouldDisplay);
        }
    }
}
