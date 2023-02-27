using BackEnd;
using PeepBo.Managers;
using PeepBo.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Data
{
    [Serializable]
    public class UserData
    {
        bool? mainTutorialPlayed = null;
        bool? dateTutorialPlayed = null;
        bool? dateOpened = null;
        string nickname = null;
        int? diamond = null;
        int? heart = null;
        int? key = null;
        string lang = "KOR";

        public bool? MainTutorialPlayed
        {
            get => mainTutorialPlayed;
            set
            {
                if (MainTutorialPlayed.HasValue) { Update(nameof(mainTutorialPlayed), value); }

                mainTutorialPlayed = value;
            }
        }

        public bool? DateTutorialPlayed
        {
            get => dateTutorialPlayed;
            set
            {
                if (DateTutorialPlayed.HasValue) { Update(nameof(dateTutorialPlayed), value); }

                dateTutorialPlayed = value;
            }
        }

        public bool? DateOpened
        {
            get => dateOpened;
            set
            {
                if (DateOpened.HasValue) { Update(nameof(dateOpened), value); }

                dateOpened = value;
            }
        }

        public string Nickname
        {
            get => nickname;
            set
            {
                if (!string.IsNullOrEmpty(nickname)) { Update(nameof(nickname), value); }

                nickname = value;
            }
        }

        public int? Diamond = 0;
        public int? Heart = 0;
        public int? Key = 0;
        public string Lang = "KOR";

        public PlayData playData = new PlayData();


        // 게임 내 상호작용으로 인한 변경이므로 서버와 동기화
        private void Update<T>(string paramName, T toUpdateValue)
        {
            Param param = new Param();
            param.Add(paramName, toUpdateValue);

            Backend.GameData.Update(GameManager.UserDataManager.UserDataTableName, new Where { }, param);
        }
    }

    [Serializable]
    public class UserDataEntity
    {
        public bool mainTutorialPlayed;
        public bool dateTutorialPlayed;
        public bool dateOpened;
        public string nickname;
        public int diamond;
        public int heart;
        public int key;
        public string lang;

        public UserData CreateUserData()
        {
            return new UserData 
            { 
                MainTutorialPlayed = mainTutorialPlayed,
                DateTutorialPlayed = dateTutorialPlayed,
                DateOpened = dateOpened,
                Nickname = nickname 
            };
        }
    }

    [Serializable]
    public class PlayData
    {

    }
}