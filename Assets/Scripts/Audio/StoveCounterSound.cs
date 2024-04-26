using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KitchenChaos.Interactions;

namespace KitchenChaos.Audio
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] StoveCounter _stoveCounter;
        [SerializeField] SoundPlayer _soundPlayer;
        SO_SoundConfig _soundConfig;

        bool _playWarningSound;
        float _warningSoundTimer;
        Vector3 _thisPosition;

        void Start()
        {
            _soundConfig = _soundPlayer.SoundConfig;
            _thisPosition = _stoveCounter.gameObject.transform.position;

            _stoveCounter.OnProgressChanged += _stoveCounter_OnProgressChanged;
            StoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        }
        
        void Update()
        {
            if (!_playWarningSound) return;

            _warningSoundTimer -= Time.deltaTime;

            if (_warningSoundTimer <= 0)
            {
                float warningSoundTimerMax = .2f;
                _warningSoundTimer = warningSoundTimerMax;

                _soundConfig.PlaySingleSound(SoundType.BurnWarning, _thisPosition);
            }

        }

        void _stoveCounter_OnProgressChanged(float progress)
        {
            float burnShowProgressAmount = .5f;
            _playWarningSound = _stoveCounter.IsFried() && progress >= burnShowProgressAmount;
        }

        void StoveCounter_OnStateChanged(StoveCounter.State state, StoveCounter counter)
        {
            if (counter != _stoveCounter) return;

            bool shouldPlay = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;

            if (shouldPlay)
                _soundConfig.PlayLoopingSound(SoundType.StoveSizzle, _stoveCounter.gameObject);
            else
                _soundConfig.StopPlayingSound(_stoveCounter.gameObject);
        }

        void OnDestroy()
        {
            _stoveCounter.OnProgressChanged -= _stoveCounter_OnProgressChanged;
            StoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
        }
    }
}
