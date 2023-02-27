using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Managers
{
    public class DateManager
    {
        public void UnLockDate()
        {
            GameManager.UserDataManager.UserData.DateOpened = true;
            GameManager.UserDataManager.UserPlayData.unlockedEpisodeList.AddRange(new List<string>{"G106", "D106" });
            Param param = new Param();
            param.Add(GameManager.UserDataManager.UserPlayedEpisodeDataDictName, GameManager.UserDataManager.UserPlayData);

            Backend.GameData.Update(GameManager.UserDataManager.UserPlayedEpisodeDataTableName, new Where(), param);
        }

        public void CompleteDateTutorial() => GameManager.UserDataManager.UserData.DateTutorialPlayed = true;

        public bool CheckShowingDateTutorial()
        {
            if (!GameManager.UserDataManager.UserData.DateOpened.HasValue || !GameManager.UserDataManager.UserData.DateTutorialPlayed.HasValue)
                return false;
            return GameManager.UserDataManager.UserData.DateOpened.Value && !GameManager.UserDataManager.UserData.DateTutorialPlayed.Value;
        }

        public bool IsDateOpened() => GameManager.UserDataManager.UserData.DateOpened == true;
    }
}