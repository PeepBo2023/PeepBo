using UnityEngine;
using PeepBo.Managers;
using PeepBo.UI.Scene;
using BackEnd;
using System.Collections.Generic;

namespace PeepBo.Scene
{ 
    public class MainScene : BaseScene
    {
        [SerializeField] AudioSource bgmSource;

        private void Start()
                => Init();

        protected override void Init()
        {
            base.Init();

            GameManager.Audio.SetMainSceneBGM(bgmSource);

            GameManager.UI.ShowSceneUI<UI_MainScene>();

            BackendReturnObject bro = Backend.BMember.GetUserInfo();
            if (bro.IsSuccess())
            {
                Debug.Log(bro.GetReturnValuetoJSON().ToJson().ToString());
            }
            var bgm = GetComponent<AudioSource>();
            bgm.PlayOneShot(bgm.clip);
            Test();
        }

        private void Test()
		{
            /*
            Param lunch = new Param();
            lunch.Add("how much", 332);
            lunch.Add("when", "yesterday");
            lunch.Add("what", "eat chocolate");

            Dictionary<string, int> dic = new Dictionary<string, int>
{
    { "dic1", 1 },
    { "dic4", 2 },
    { "dic2", 4 }
};

            Dictionary<string, string> dic2 = new Dictionary<string, string>
{
    { "mm", "j" },
    { "nn", "n" },
    { "dd", "2" }
};
            
            string[] list = { "a", "b" };
            int[] list2 = { 400, 500, 600 };

            Param param = new Param();
            param.Add("¿Ã∏ß", "cheolsu");
            param.Add("score", 99);
            param.Add("lunch", lunch);
            param.Add("dic_num", dic);
            param.Add("dic_string", dic2);
            param.Add("list_string", list);
            param.Add("list_num", list2);
            */

            /*
            UserPlayData userPlayData = new UserPlayData();
            PlayedEpisodeData playedEpisodeData = new PlayedEpisodeData();
            playedEpisodeData.choicedData.Add(0, 0);
            playedEpisodeData.choicedData.Add(1, 2);
            PlayedEpisodeData playedEpisodeData2 = new PlayedEpisodeData();
            playedEpisodeData2.choicedData.Add(0, 1);
            playedEpisodeData2.choicedData.Add(1, 1);
            userPlayData.playedEpisodeDataDict.Add(101, playedEpisodeData);
            userPlayData.playedEpisodeDataDict.Add(102, playedEpisodeData2);
            Param param = new Param();
            param.Add("UserPlayDataDict", userPlayData);

            Backend.GameData.Insert("UserPlayData", param);
            */
            /*
            UserPlayData userPlayData = new UserPlayData();
            PlayedEpisodeData playedEpisodeData = new PlayedEpisodeData();
            playedEpisodeData.choicedList.Add(0);
            playedEpisodeData.choicedList.Add(2);
            PlayedEpisodeData playedEpisodeData2 = new PlayedEpisodeData();
            playedEpisodeData2.choicedList.Add(1);
            playedEpisodeData2.choicedList.Add(0);
            userPlayData.playedEpisodeDataDict.Add("101", playedEpisodeData);
            userPlayData.playedEpisodeDataDict.Add("102", playedEpisodeData2);
            Param param = new Param();
            param.Add("UserPlayDataDict", userPlayData);

            Backend.GameData.Insert("UserPlayData", param);
            */
        }
    }
}
