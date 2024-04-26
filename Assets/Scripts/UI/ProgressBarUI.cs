using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Interactions.UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] GameObject _progressTracker;
        [SerializeField] Image _barImage;

        IHasProgress _progress;

        void Start()
        {
            _progress = _progressTracker.GetComponent<IHasProgress>();

            if (_progress == null)
                Debug.LogError(_progressTracker + " does not have a component that implements IHasProgress!");

            _progress.OnProgressChanged += _progress_OnProgressChanged;

            _barImage.fillAmount = 0f;
            DisplayProgressBar(false);
        }

        void _progress_OnProgressChanged(float obj)
        {
            _barImage.fillAmount = obj;

            if (obj == 0f || obj == 1f)
                DisplayProgressBar(false);
            else
                DisplayProgressBar(true);
        }

        void DisplayProgressBar(bool shouldDisplay)
        {
            gameObject.SetActive(shouldDisplay);
        }

        void OnDestroy()
        {
            _progress.OnProgressChanged -= _progress_OnProgressChanged;
        }
    }
}
