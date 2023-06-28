using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Sound;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts.Sound
{
    public class SoundControl : MonoBehaviour
    {
        [SerializeField] private AudioSource ambientSource;

        [SerializeField] private List<AudioSource> effectsSources;

        [SerializeField] private bool playAmbientOnAwake = true;

        public AudioInfo audioInfo;

        
        private PlayerData playerData;
        [Inject]
        private void Construct(PlayerData playerData)
        {
            this.playerData = playerData;

        }

        private void Start()
        {
            SyncSoundWithSettings();
            
            if (playAmbientOnAwake)
            {
                PlayAmbient(AudioDefault.Background);
            }
        }


        public void PlayAmbient(AudioDefault audioId, bool loop = true, float delayBeforeStart = 0,
            bool fromFade = false, float durationForFade = 1)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                PlayAmbient(audioClip, loop, delayBeforeStart, fromFade, durationForFade);
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId);
            }
        }

        public void PlayAmbient(AudioData audioData, bool loop = true, float delayBeforeStart = 0,
            bool fromFade = false, float durationForFade = 1)
        {
            if (ambientSource.clip != audioData.clip)
            {
                if (fromFade)
                {
                    ambientSource.DOFade(0, durationForFade / 2f).OnComplete(() =>
                    {
                        ambientSource.clip = audioData.clip;
                        ambientSource.PlayDelayed(delayBeforeStart);
                        ambientSource.loop = loop;
                        ambientSource.DOFade(playerData.settings.Value.soundVolume, durationForFade / 2f).SetDelay(delayBeforeStart);

                    });
                }
                else
                {
                    if (ambientSource.isPlaying)
                    {
                        ambientSource.Stop();
                    }

                    ambientSource.clip = audioData.clip;
                    ambientSource.volume = playerData.settings.Value.soundVolume;
                    ambientSource.PlayDelayed(delayBeforeStart);
                    ambientSource.loop = loop;
                }

            }
            else
            {
                Debug.LogWarning("Same clip is already playing");
            }
        }

        public void PlayEffect(string audioId)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                foreach (var effectsSource in effectsSources)
                {
                    if (!effectsSource.isPlaying)
                    {
                        effectsSource.volume = playerData.settings.Value.soundVolume;
                        effectsSource.PlayOneShot(audioClip.clip);
                        break;
                    }
                }
                
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId.ToString());
            }
        }

        public void PlayEffect(AudioDefault audioId)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                foreach (var effectsSource in effectsSources)
                {
                    if (!effectsSource.isPlaying)
                    {
                        effectsSource.volume = playerData.settings.Value.soundVolume;
                        effectsSource.PlayOneShot(audioClip.clip);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId.ToString());
            }
        }
        
        public void PlayEffect(AudioDefault audioId, bool loop)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                foreach (var effectsSource in effectsSources)
                {
                    if (!effectsSource.isPlaying)
                    {
                        effectsSource.volume = playerData.settings.Value.soundVolume;
                        if (loop)
                        {
                            effectsSource.loop = true;
                            effectsSource.volume = playerData.settings.Value.soundVolume;
                            effectsSource.clip = audioClip.clip;
                            effectsSource.Play();
                        }
                        else
                        {
                            effectsSource.PlayOneShot(audioClip.clip);
                        }
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId.ToString());
            }
        }
        
        public void PlayEffect(AudioDefault audioId, float duration)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                foreach (var effectSource in effectsSources)
                {
                    if (!effectSource.isPlaying)
                    {
                        StartCoroutine(PlayLongEffect(effectSource, audioClip, duration));
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId);
            }
        }

        IEnumerator PlayLongEffect(AudioSource effectSource, AudioData audioClip, float duration)
        {
            effectSource.loop = true;
            effectSource.volume = playerData.settings.Value.soundVolume;
            effectSource.clip = audioClip.clip;
            effectSource.Play();
            yield return new WaitForSeconds(duration);
            effectSource.Stop();
            effectSource.loop = false;
        }

        public void StopEffect(AudioDefault audioId)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);

            foreach (var effectSource in effectsSources)
            {
                if (effectSource.clip != null && effectSource.clip == audioClip.clip)
                {
                    effectSource.Stop();
                    effectSource.clip = null;
                    effectSource.loop = false;
                }
            }
        }

        public void SyncSoundWithSettings()
        {
            UpdateVolume();
        }

        private void UpdateVolume()
        {
            ambientSource.volume = playerData.settings.Value.soundVolume;

            foreach (var effectsSource in effectsSources)
            {
                effectsSource.volume = playerData.settings.Value.soundVolume;
            }
        }
    }
}