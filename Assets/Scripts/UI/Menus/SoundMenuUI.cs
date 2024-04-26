using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Audio.UI
{
    public class SoundMenuUI : MonoBehaviour
    {
        [SerializeField] Button _setDefaultsButton;
        [SerializeField] Button _backButton;
        [SerializeField] Slider _musicVolumeSlider;
        [SerializeField] Slider _soundVolumeSlider;

        SoundPlayer _soundPlayer;
        MusicPlayer _musicPlayer;
        SO_SoundConfig _levelsSoundConfig;

        void Awake()
        {
            _soundPlayer = FindObjectOfType<SoundPlayer>();
            _musicPlayer = FindObjectOfType<MusicPlayer>();
            _levelsSoundConfig = _soundPlayer.SoundConfig;

            SetStartingSliderValues();

            _setDefaultsButton.onClick.AddListener(() =>
            {
                SetDefaultValues();
            });

            _backButton.onClick.AddListener(() =>
            {
                SaveVolumeValues();
            });
   
        }

        void Start()
        {
            gameObject.SetActive(false);
        }


        void Update()
        {
            SetVolume();
        }
        
        void SetStartingSliderValues()
        {
            if (SoundPlayerPrefs.CheckForMusicVolumeKey())
                _musicVolumeSlider.value = SoundPlayerPrefs.GetMusicVolume();
            else
                _musicVolumeSlider.value = _musicPlayer.DefaultVolume;


            if (SoundPlayerPrefs.CheckForSoundVolumeKey())
                _soundVolumeSlider.value = SoundPlayerPrefs.GetSoundVolume();
            else
                _soundVolumeSlider.value = _levelsSoundConfig.DefaultVolume;
        }
        
        void SetVolume()
        {
            if (_musicPlayer)
                _musicPlayer.Volume = _musicVolumeSlider.value;

            if (_soundPlayer)
                _levelsSoundConfig.Volume = _soundVolumeSlider.value;
        }

        void SetDefaultValues()
        {
            _musicVolumeSlider.value = _musicPlayer.DefaultVolume;
            _musicPlayer.Volume = _musicPlayer.DefaultVolume;
            _soundVolumeSlider.value = _levelsSoundConfig.DefaultVolume;
            _levelsSoundConfig.Volume = _levelsSoundConfig.DefaultVolume;
        }

        void SaveVolumeValues()
        {
            SoundPlayerPrefs.SetMusicVolume(_musicVolumeSlider.value);
            SoundPlayerPrefs.SetSoundVolume(_soundVolumeSlider.value);
        }
    }
}
