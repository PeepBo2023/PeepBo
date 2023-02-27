using PeepBo.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeepBo.Managers
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        public static GameManager Instance { get { Init(); return instance; } }


        ResourceManager resourceManager = new ResourceManager();
        UIManager uiManager = new UIManager();
        RoomManager roomManager = new RoomManager();
        CommandManager commandManager = new CommandManager();
        AudioManager audioManager = new AudioManager();
        HogamManager hogamManager = new HogamManager();

        AccountManager accountManager = new AccountManager();
        UserDataManager userDataManager = new UserDataManager();
        UpdateManager updateManager;
        EpisodeManager episodeManager = new EpisodeManager();
        DataManager dataManager = new DataManager();
        ChoiceManager choiceManager = new ChoiceManager();
        DateManager dateManager = new DateManager();
        SettingManager settingManager = new SettingManager();


        public static ResourceManager Resource { get => Instance.resourceManager; }
        public static UIManager UI { get => Instance.uiManager; }
        public static RoomManager Room { get => Instance.roomManager; }
        public static CommandManager Command { get => Instance.commandManager; }
        public static AudioManager Audio { get => Instance.audioManager; }
        public static HogamManager Hogam { get => Instance.hogamManager; }

        public static AccountManager Account { get => Instance.accountManager; }
        public static UserDataManager UserDataManager { get => Instance.userDataManager; }
        public static UpdateManager Update { get => Instance.updateManager; }
        public static EpisodeManager Episode { get => Instance.episodeManager; }
        public static DataManager Data { get => Instance.dataManager; }
        public static ChoiceManager Choice { get => Instance.choiceManager; }
        public static DateManager Date { get => Instance.dateManager; }
        public static SettingManager Setting { get => Instance.settingManager; }



        public static string DummyEpisode;

        public static Dictionary<string, string> EpisodeTitleDict = new Dictionary<string, string>() 
        { 
            { "000", "프롤로그" },
            { "101", "저는 그냥 관광객인데요?" },
            { "102", "가이드" },
            { "103", "해록도의 중심, 선락원" },
            { "104", "비밀장소" },
            { "105", "내가 이곳에 온 이유" },
            { "106", "환영회" },
            { "107", "천사의 집" },
            { "108", "화관의 주인" },
            { "109", "담벼락의 붉은 글씨" },
            { "110", "흩날리는 동백" },
            { "111", "혼란 그리고 밤" },
            { "D106", "다함이와 데이트" },
            { "G106", "고결과 데이트" },
        };

        public static void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<GameManager>();
                }
                instance = go.GetComponent<GameManager>();

                InitInputManager();
                InitBackendErrorManager();
                InitUpdateManager();
                InitSettingManager();
                InitAudioManager();

                DontDestroyOnLoad(instance.gameObject);
            }
        }

        public static void LoadMainScene(Action BeforeComplete = null)
        {
            var scene = SceneManager.LoadSceneAsync("MainScene");
        }

        public static void LoadMainSceneAndEpisodePopup(Action BeforeComplete = null)
        {
            var scene = SceneManager.LoadSceneAsync("MainScene");
            scene.completed += ShowEpisodePopup;
            /*
            scene.allowSceneActivation = false;
            Debug.Log("B");
            scene.completed += (_) =>
            {
                Debug.Log("A");
                BeforeComplete?.Invoke();
                scene.allowSceneActivation = true;
                ShowEpisodePopup(_);
            };
            */
        }

		private static void ShowEpisodePopup(AsyncOperation obj)
		{
            GameManager.UI.ShowPopupUI<UI_EpisodePopup>();
		}

		private static void InitInputManager()
        {
            instance.gameObject.AddComponent<InputManager>();
        }

        private static void InitBackendErrorManager()
        {
            Instance.gameObject.AddComponent<BackendErrorManager>();
        }

        private static void InitUpdateManager()
        {
            Instance.updateManager = Instance.gameObject.AddComponent<UpdateManager>();
        }

        private static void InitSettingManager()
        {
            Instance.settingManager.InitSettingManager();
        }

        private static void InitAudioManager()
        {
            Instance.audioManager.InitAudioManager();
        }
    }
}
