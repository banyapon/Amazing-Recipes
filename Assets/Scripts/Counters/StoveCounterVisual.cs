using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions.Visual
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] StoveCounter _stoveCounter;
        [SerializeField] GameObject _stoveOnVisual;
        [SerializeField] GameObject _particles;

        void Start()
        {
            StoveCounter.OnStateChanged += _stoveCounter_OnStateChanged;
        }

        void _stoveCounter_OnStateChanged(StoveCounter.State state, StoveCounter counter)
        {
            if (_stoveCounter != counter) return;

            bool showVisual = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;

            _stoveOnVisual.SetActive(showVisual);
            _particles.SetActive(showVisual);
        }

        void OnDestroy()
        {
            StoveCounter.OnStateChanged -= _stoveCounter_OnStateChanged;
        }
    }
}
