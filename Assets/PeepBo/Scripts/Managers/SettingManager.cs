using PeepBo.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Managers
{
    public class SettingManager
    {
        public SettingData SettingData { get; private set; }
        public UserData UserData { get; private set; }

        public void InitSettingManager() // by UserDataManager.LoadUserLocalData()
        {
            LoadOrCreateSettingData();

            UserData = GameManager.UserDataManager.UserData;
        }

        private void LoadOrCreateSettingData() // Application.persistentDataPath에 저장된 데이터 불러오기
        {
            SettingData = SettingData.LoadOrCreateUserSettingData();
        }

        public void SaveSetting(float bgm, float sfx, float voice, bool vibrationOn, bool notificationOn)
        {
            SettingData.bgmSound = bgm;
            SettingData.sfxSound = sfx;
            SettingData.voiceSound = voice;
            SettingData.vibrationOn = vibrationOn;
            SettingData.notificationOn = notificationOn;

            SettingData.Save();
        }

        public void TryUpdateName(string newName, Action OnSuccess, Action OnFail)
		{
            if (!string.IsNullOrEmpty(newName))
            {
                UserData.Nickname = newName;
                OnSuccess?.Invoke();
            }
            else
                OnFail?.Invoke();
		}
    }
}