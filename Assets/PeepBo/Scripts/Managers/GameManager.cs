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
            { "000", "���ѷα�" },
            { "101", "���� �׳� �������ε���?" },
            { "102", "���̵�" },
            { "103", "�طϵ��� �߽�, ������" },
            { "104", "������" },
            { "105", "���� �̰��� �� ����" },
            { "106", "ȯ��ȸ" },
            { "107", "õ���� ��" },
            { "108", "ȭ���� ����" },
            { "109", "�㺭���� ���� �۾�" },
            { "110", "�𳯸��� ����" },
            { "111", "ȥ�� �׸��� ��" },
            { "D106", "�����̿� ����Ʈ" },
            { "G106", "���� ����Ʈ" },
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
