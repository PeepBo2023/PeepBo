using BackEnd;
using PeepBo.Data;
using PeepBo.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeepBo.Managers
{
    public class UserDataManager
    {
        public readonly string UserDataTableName = "UserInfo";
        public readonly string UserPlayedEpisodeDataTableName = "PlayedEpisodeData";
        public readonly string UserPlayedEpisodeDataDictName = "PlayedEpisodeDataDict";

        public UserData UserData { get; private set; } = new UserData();
        public UserPlayData UserPlayData { get; private set; } = new UserPlayData();

        public void TryLoadUserData()
        {
            LoadUserData();
            LoadUserPlayData();
        }

        private void LoadUserData() // 뒤끝에 저장된 유저 Common 데이터 불러오기
        {
            var userDataBRO = Backend.GameData.GetMyData(UserDataTableName, new Where());

            if (IsFirstUser(userDataBRO)) // 테이블 내에 유저 데이터 존재 x, 새로 가입한 유저
            {
                // 초기화 진행
                userDataBRO = CreateDefaultUserData();
            }

            var userDataJson = userDataBRO.FlattenRows()[0].ToJson();
            var userDataEntity = LitJson.JsonMapper.ToObject<UserDataEntity>(userDataJson);

            UserData = userDataEntity.CreateUserData();

            // 로컬 함수
            BackendReturnObject CreateDefaultUserData()
            {
                Backend.GameData.Insert(UserDataTableName); // Schema에 정의된 Default 데이터 생성
                return Backend.GameData.GetMyData(UserDataTableName, new Where());
            }
        }
        
        private void LoadUserPlayData()
		{
            var userPlayDataBRO = Backend.GameData.GetMyData(UserPlayedEpisodeDataTableName, new Where());

            if (IsFirstUser(userPlayDataBRO)) // 처음 가입한 유저
            {
                Debug.Log("Data Create");
                UserPlayData = new UserPlayData();
                UserPlayData.unlockedEpisodeList.Add("000");

                // TODO : 버닝 비버용 테스트 코드

                //테스트 코드
                /*
                UserPlayData.unlockedEpisodeList.AddRange(new List<string> { "101","102","103","104","105","106","107","108","109","110","111" });
                UserPlayData.playedEpisodeDataDict.Add("000", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("101", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("102", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("103", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("104", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("105", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("106", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("107", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("108", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("109", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("110", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("111", new PlayedEpisodeData { episodeCompleted = true });
                */
                UserPlayData.unlockedEpisodeList.AddRange(new List<string> { "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "201", "202", "203", "204", "205", "206", "207", "208", "209", "210" });
                UserPlayData.playedEpisodeDataDict.Add("000", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("101", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("102", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("103", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("104", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("105", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("106", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("107", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("108", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("109", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("110", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("111", new PlayedEpisodeData { episodeCompleted = true });

                UserPlayData.playedEpisodeDataDict.Add("201", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("202", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("203", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("204", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("205", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("206", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("207", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("208", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("209", new PlayedEpisodeData { episodeCompleted = true });
                UserPlayData.playedEpisodeDataDict.Add("210", new PlayedEpisodeData { episodeCompleted = true });

                GameManager.Date.UnLockDate();
                

                Param param = new Param();
                param.Add(UserPlayedEpisodeDataDictName, UserPlayData);

                Backend.GameData.Insert(UserPlayedEpisodeDataTableName, param);
                return;
            }

            var userPlayDataJson = userPlayDataBRO.FlattenRows()[0][UserPlayedEpisodeDataDictName].ToJson();

            UserPlayData = LitJson.JsonMapper.ToObject<UserPlayData>(userPlayDataJson);
            
            /*
            foreach(var a in UserPlayData.playedEpisodeDataDict)
			{
                Debug.Log(a.Key);
                Debug.Log(a.Value.roomCompleted);
                foreach(var b in a.Value.choicedList)
				{
                    Debug.Log(b);
                }
			}
            Debug.Log(UserPlayData);
            Debug.Log(UserPlayData.ToString());
            */
            //Debug.Log(userPlayDataJson.ToString());
            
        }

        private bool IsFirstUser(BackendReturnObject bro) => bro.FlattenRows().Count == 0;
    }
}