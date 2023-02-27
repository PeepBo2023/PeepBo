using System;
using System.IO;
using UnityEngine;

namespace PeepBo.Data
{
    [Serializable]
    public class SettingData
    {
        public static readonly string seettingDataJson = Application.persistentDataPath + "/UserSettingData.json";

        public float bgmSound = 0.5f;
        public float sfxSound = 0.5f;
        public float voiceSound = 0.5f;
        public bool vibrationOn = true;
        public bool notificationOn = true;

        public void Save()
        {
            File.WriteAllText(seettingDataJson, JsonUtility.ToJson(this, true));
        }

        public static SettingData LoadOrCreateUserSettingData()
        {
            if (File.Exists(seettingDataJson))
            {
                var json = File.ReadAllText(seettingDataJson);
                return JsonUtility.FromJson<SettingData>(json);
            }
            else
            {
                SettingData userSettingData = new SettingData();
                File.WriteAllText(seettingDataJson, JsonUtility.ToJson(userSettingData, true));
                return userSettingData;
            }
        }
    }
}