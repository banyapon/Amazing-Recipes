using System;
using UnityEngine;

namespace KitchenChaos.Audio
{
    [CreateAssetMenu()]
    public class SO_SoundConfig : ScriptableObject
    {
        [SerializeField] SoundConfig[] _sounds;
        [SerializeField] float _defaultVolume = 1f;
        public float DefaultVolume => _defaultVolume;

        float _volume;

        public float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        [System.Serializable]
        class SoundConfig
        {
            public SoundType SoundType;
            public AudioClip[] AudioClips;
        }

        void Awake()
        {
            if (SoundPlayerPrefs.CheckForSoundVolumeKey())
                _volume = SoundPlayerPrefs.GetSoundVolume();
            else
                _volume = _defaultVolume;
        }

        public void PlaySingleSound(SoundType soundType, Vector3 position)
        {
            SoundConfig sc = FindSoundConfig(soundType);
            if (sc == null) return;

            AudioClip clip;

            if (sc.AudioClips.Length > 1)
                clip = sc.AudioClips[UnityEngine.Random.Range(0, sc.AudioClips.Length)];
            else
                clip = sc.AudioClips[0];

            AudioSource.PlayClipAtPoint(clip, position, _volume);
        }

        public void PlayLoopingSound(SoundType soundType, GameObject obj)
        {
            SoundConfig sc = Array.Find(_sounds, config => config.SoundType == soundType);
            if (sc == null) return;

            if (sc.AudioClips.Length > 1)
            {
                Debug.LogWarning("Sounds in an array shouldn't be looping!");
                return;
            }

            if (!obj.TryGetComponent(out AudioSource source))
            {
                source = obj.AddComponent<AudioSource>();
                source.clip = sc.AudioClips[0];
                source.spatialBlend = 1f;
                source.loop = true;
            }

            source.volume = _volume;
            source.Play();
        }

        public void StopPlayingSound(GameObject obj)
        {
            if (obj.TryGetComponent(out AudioSource source))
                source.Stop();
            else
                Debug.LogWarning("The object " + obj + " has no AudioSource component");
        }

        SoundConfig FindSoundConfig(SoundType soundType)
        {
            SoundConfig sc = Array.Find(_sounds, config => config.SoundType == soundType);

            if (sc == null)
            {
                Debug.LogWarning("There is no sound configuration of the type " + soundType);
                return null;
            }

            return sc;
        }
    }
}
