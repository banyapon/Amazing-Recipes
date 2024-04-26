using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] float _defaultVolume = 1f;
        public float DefaultVolume => _defaultVolume;

        AudioSource _audioSource;

        public float Volume
        {
            get { return _audioSource.volume; }
            set { _audioSource.volume = value; }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (SoundPlayerPrefs.CheckForMusicVolumeKey())
                _audioSource.volume = SoundPlayerPrefs.GetMusicVolume();
            else
                _audioSource.volume = _defaultVolume;
        }

    }
}
