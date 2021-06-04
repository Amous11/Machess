using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    [System.Serializable]
    public class DictionarySounds : UnitySerializedDictionary<eSounds, SoundData>
    {
    }
    public class SoundManagerBase : Singleton<SoundManager>
    {
        [Header("Audio Sources")]
        public AudioSource MusicSource;
        public AudioSource SFXGameAudioSource;
        public AudioSource SFXUIAudioSource;
        [SerializeField]
        private DictionarySounds SoundEffects;
        public void PlaySoundFX(eSounds i_Sound)
        {
            if(StorageManager.Instance.IsSoundEffectsOn)
            {
                if(SoundEffects.ContainsKey(i_Sound))
                {
                    SFXGameAudioSource.PlayOneShot(SoundEffects[i_Sound].AudioClip, SoundEffects[i_Sound].Volume);
                }
            }
        }
        public virtual void PlayMusic(AudioClip i_AudioClip)
        {
            MusicSource.clip = i_AudioClip;
            PlayMusic();
        }
        public virtual void PlayMusic()
        {
            MusicSource.Play();
        }
    }
    [Serializable]
    public class SoundData
    {
        public AudioClip AudioClip;
        [Range(0f, 1f)]
        public float Volume = .5f;
       
    }
}
