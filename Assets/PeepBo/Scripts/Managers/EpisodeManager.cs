using BackEnd;
using Naninovel;
using PeepBo.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeepBo.Managers
{
    public enum EpisodeState
    {
        New,
        Playing,
        NotPlayed,
        Played,
        Locked,
    }

    public class EpisodeManager
    {
        public string NewEpisode { get; set; }

        public void InitEpisodeManager()
		{
            GameManager.UI.ShowLoadingPopup();

            InitNaniEngine();

            Debug.Log("Init");

            if (Engine.Initialized) StartEpisode();
            else Engine.OnInitializationFinished += StartEpisode;
        }

        private async void InitNaniEngine() => await RuntimeInitializer.InitializeAsync();

        private async void StartEpisode()
        {
            Engine.OnInitializationFinished -= StartEpisode; // 무조건 넣어야 함

            var player = Engine.GetService<IScriptPlayer>();
            var stateManager = Engine.GetService<IStateManager>();

            var episode = GameManager.DummyEpisode;
            var episodeState = GetEpisodeState(episode);
            if (episodeState == EpisodeState.New || episodeState == EpisodeState.NotPlayed) // 유저가 에피소드 난생 처음 플레이 할때
            {
                Debug.Log("First Played");
                var playedEpisodeDict = GameManager.UserDataManager.UserPlayData.playedEpisodeDataDict;
                playedEpisodeDict.Add(episode, new PlayedEpisodeData());

                Param param = new Param();
                param.Add(GameManager.UserDataManager.UserPlayedEpisodeDataDictName, GameManager.UserDataManager.UserPlayData);

                Backend.GameData.Update(GameManager.UserDataManager.UserPlayedEpisodeDataTableName, new Where(), param);
            }

            player.OnPreloadProgress += HideLoadingPopup;

            await player.PreloadAndPlayAsync($"Script{episode}");

            player.OnPreloadProgress -= HideLoadingPopup;



            void HideLoadingPopup(float progress)
            {
                if (progress == 1f)
                    GameManager.UI.HideLoadingPopup();
            }
        }

        public EpisodeState GetEpisodeState(string episode)
        {
            var userPlayData = GameManager.UserDataManager.UserPlayData;
            var userUnlockedEpisodeList = userPlayData.unlockedEpisodeList;
            var playedEpisodeDict = userPlayData.playedEpisodeDataDict;

            playedEpisodeDict.TryGetValue(episode, out var playedData);

            if (playedData == null)
            {
                foreach(var unlockedEpisode in userUnlockedEpisodeList)
                {
                    if (unlockedEpisode == episode)
                    {
                        if (NewEpisode == episode)
                            return EpisodeState.New;
                        return EpisodeState.NotPlayed;
                    }
                }
                return EpisodeState.Locked;
            }
            if (playedData.episodeCompleted)
                return EpisodeState.Played;
            return EpisodeState.Playing;
        }

        public void OnEpisodeEnd() 
        {
            GameManager.UI.ShowLoadingPopup();

            TryUpdateUserPlayedEpisodeData();

            TryUnLockDate(GameManager.DummyEpisode);

            Release();

            Engine.Destroy();

            if (GameManager.DummyEpisode[0] == 'D' || GameManager.DummyEpisode[0] == 'Y' || GameManager.DummyEpisode[0] == 'W' || GameManager.DummyEpisode[0] == 'G')
                GameManager.LoadMainScene(GameManager.UI.HideLoadingPopup);
            else
                GameManager.LoadMainSceneAndEpisodePopup(GameManager.UI.HideLoadingPopup);
        }

        private void TryUpdateUserPlayedEpisodeData()
        {
            string currendPlayedEpisode = GameManager.DummyEpisode;
            if (GetEpisodeState(currendPlayedEpisode) == EpisodeState.Played)
                return;

            // TODO : 버닝비버 임시 코드
            switch (currendPlayedEpisode)
            {
                case "000":
                    NewEpisode = "101";
                    break;
                case "101":
                    NewEpisode = "102";
                    break;
                case "102":
                    NewEpisode = "103";
                    break;
                case "103":
                    NewEpisode = "104";
                    break;
                case "104":
                    NewEpisode = "105";
                    break;
                case "105":
                    NewEpisode = "106";
                    break;
                case "106":
                    NewEpisode = "107";
                    break;
                case "107":
                    NewEpisode = "108";
                    break;
                case "108":
                    NewEpisode = "109";
                    break;
                case "109":
                    NewEpisode = "110";
                    break;
                case "110":
                    NewEpisode = "111";
                    break;
                case "111":
                    NewEpisode = null;
                    break;
            }

            if (!(GameManager.DummyEpisode[0] == 'D' || GameManager.DummyEpisode[0] == 'Y' || GameManager.DummyEpisode[0] == 'W' || GameManager.DummyEpisode[0] == 'G'))
                GameManager.UserDataManager.UserPlayData.unlockedEpisodeList.Add(NewEpisode);
            GameManager.UserDataManager.UserPlayData.playedEpisodeDataDict.TryGetValue(currendPlayedEpisode, out var playedEpisodeData);
            playedEpisodeData.episodeCompleted = true;

            Param param = new Param();
            param.Add(GameManager.UserDataManager.UserPlayedEpisodeDataDictName, GameManager.UserDataManager.UserPlayData);

            Backend.GameData.Update(GameManager.UserDataManager.UserPlayedEpisodeDataTableName, new Where(), param);
        }

        private void TryUnLockDate(string currentPlayedEpisode)
        {
            if (currentPlayedEpisode != "106")
                return;

            if (GameManager.Date.IsDateOpened())
                return;

            GameManager.Date.UnLockDate();
        }

        private void Release()
        {
            GameManager.Choice.Release();
        }
    }
}