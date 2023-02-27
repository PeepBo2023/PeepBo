using PeepBo.Data;
using PeepBo.Scene;
using PeepBo.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Managers
{
    public class AudioManager
    {
        private AudioClip clickSound;
        private AudioSource mainAudioSource;
        private AudioSource clickAudioSource;

        AudioSource loginSceneBGM, mainSceneBGM;

        public SettingData SettingData { get; private set; }

        public void InitAudioManager()
        {
            clickAudioSource = GameManager.Instance.gameObject.AddComponent<AudioSource>();
            clickAudioSource.playOnAwake = false;

            clickSound = GameManager.Resource.Load<AudioClip>("Audios/Å¬¸¯");

            SettingData = GameManager.Setting.SettingData;

            UpdateSFXVolume(SettingData.sfxSound);
        }

        public void PlayClickSound()
		{
            clickAudioSource.PlayOneShot(clickSound);
		}

        public void UpdateBGMVolume(float volume)
        {
            if (loginSceneBGM != null)
                loginSceneBGM.volume = volume;
            if (mainSceneBGM != null)
                mainSceneBGM.volume = volume;
        }

        public void UpdateSFXVolume(float volume)
        {
            clickAudioSource.volume = volume;
        }

        public void SetLoginSceneBGM(AudioSource target)
        {
            loginSceneBGM = target;
            loginSceneBGM.volume = SettingData.bgmSound;
        }

        public void SetMainSceneBGM(AudioSource target)
        {
            mainSceneBGM = target;
            mainSceneBGM.volume = SettingData.bgmSound;
        }
    }
}
