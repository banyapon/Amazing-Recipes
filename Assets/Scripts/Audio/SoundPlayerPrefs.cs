using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Audio
{
    public class SoundPlayerPrefs : MonoBehaviour
    {
        const string MUSIC_VOLUME_KEY = "Music volume";
        const string SOUND_VOLUME_KEY = "Sound volume";

        public static void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }

        public static float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
        }

        public static bool CheckForMusicVolumeKey()
        {
            return PlayerPrefs.HasKey(MUSIC_VOLUME_KEY);
        }

        public static void SetSoundVolume(float volume)
        {
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, volume);
        }

        public static float GetSoundVolume()
        {
            return PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
        }

        public static bool CheckForSoundVolumeKey()
        {
            return PlayerPrefs.HasKey(SOUND_VOLUME_KEY);
        }
    }
}
