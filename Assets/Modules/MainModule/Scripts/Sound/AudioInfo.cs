using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.MainModule.Scripts.Sound
{
    [Serializable]
    public class AudioData
    {
        public string id;
        public AudioDefault audioDefault;
        public AudioClip clip;

        public AudioData(string id, AudioDefault audioDefault, AudioClip clip, float volume)
        {
            this.id = id;
            this.audioDefault = audioDefault;
            this.clip = clip;
        }
    }

    [CreateAssetMenu(fileName = "AudioInfo", menuName = "Sound/AudioInfo", order = 0)]
    public class AudioInfo : ScriptableObject
    {
        [SerializeField] private List<AudioData> data = new List<AudioData>();

        public AudioData GetAudioData(string id)
        {
            foreach (var d in data)
            {
                if (d.id == id)
                {
                    return d;
                }
            }

            return null;
        }

        public AudioData GetAudioData(AudioDefault type)
        {
            foreach (var d in data)
            {
                if (d.audioDefault == type)
                {
                    return d;
                }
            }

            return null;
        }
    }

    public enum AudioDefault
    {
        Custom,
        Background,
        ClickButton,
    }
}