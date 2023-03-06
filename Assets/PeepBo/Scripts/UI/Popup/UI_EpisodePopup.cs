using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using PeepBo.Component;
using TMPro;

namespace PeepBo.UI.Popup
{
    public class UI_EpisodePopup : UI_Popup
    {
        /*
        enum GameObjects
        {
            CloseButton,
            DummyGroup,

            ScrollRect,

            LockedImage,
            PlayingImage,
            NewImage,

            EpisodePage

            //Diary,
            //Date,
        }
        */

        //public GameObject dummyGroup;
        //public GameObject dateButton;
        //public List<GameObject> dummyEpisodeList = new List<GameObject> ();
        public ScrollRect scrollRect;

        public Image lockedImage, playingImage, newImage;

        public EpisodeComponent _tempEpisodeComponent;
        public GameObject group;

        public GameObject closeButton;

        Dictionary<string, EpisodeComponent> _currentEpisodeDict = new Dictionary<string, EpisodeComponent>();

        public TextMeshProUGUI pageClosedText;
        public TextMeshProUGUI pageOpenedText;

        public GameObject GOPageClosed;
        public GameObject GOPageOpened;

        public GameObject[] GOPages;

        public override void Init()
        {
            base.Init();
            BindObjects();

            //CheckShowingDate();

            InitEpisodeComponents();

            CompareUserPlayData();

            //OnShowPopup += CheckShowingDateTutorial;
        }

        //BindObjects에 버튼 이벤트만 남기고 나머지 제거. UI오브젝트를 public으로 변경 : 230305 박찬현
        
        private void BindObjects()
        {
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);

            AddUIEvent(GOPageClosed, OnClickPageClosed, Define.UIEvent.Click);
            AddButtonAnim(GOPageClosed);
            AddUIEvent(GOPageOpened, OnClickPageOpened, Define.UIEvent.Click);
            AddButtonAnim(GOPageOpened);


            foreach (GameObject go in GOPages)
            {
                AddUIEvent(go, (a) =>
                {
                    Debug.Log("Clicked : " + go.name);
                    
                    int.TryParse(go.name.Substring(4), out int _episodeNum);

                    ClearEpisodeComponents();
                    LoadEpisodeComponents(_episodeNum);

                    pageClosedText.text = _episodeNum.ToString() + "장";
                    pageOpenedText.text = _episodeNum.ToString() + "장";

                    GOPageOpened.SetActive(false);
                    GOPageClosed.SetActive(true);

                }, Define.UIEvent.Click);
                AddButtonAnim(go);

            }

            /*
            Bind<GameObject>(typeof(GameObjects));

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);

            dummyGroup = GetObject((int)GameObjects.DummyGroup);
            for(int i=0; i<dummyGroup.transform.childCount; i++)
                dummyEpisodeList.Add(dummyGroup.transform.GetChild(i).gameObject);

            scrollRect = GetObject((int)GameObjects.ScrollRect).GetComponent<ScrollRect>();

            lockedImage = GetObject((int)GameObjects.LockedImage).GetComponent<Image>();
            playingImage = GetObject((int)GameObjects.PlayingImage).GetComponent<Image>();
            newImage = GetObject((int)GameObjects.NewImage).GetComponent<Image>();

            episodePage = GetObject((int)GameObjects.EpisodePage).GetComponent<EpisodePage>();
            
            //dateButton = GetObject((int)GameObjects.Date);
            //AddUIEvent(dateButton, (_) => { GameManager.UI.ShowPopupUI<UI_DatePopup>(); }, Define.UIEvent.Click);
            //AddButtonAnim(dateButton);
            */
        }
        

        private void InitEpisodeComponents()
        {
            int episodePageNum = GameManager.Episode.episodePageNum;
            ClearEpisodeComponents();
            LoadEpisodeComponents(episodePageNum);
            /*
            foreach (var episode in dummyEpisodeList)
            {
                string identifier = episode.name.Substring(5, 3);
                var episodeComponet = episode.GetComponent<EpisodeComponent>();

                var episodeState = GameManager.Episode.GetEpisodeState(identifier);
                var episodeIndex = GameManager.Episode.episodePage;
                episodeComponet.InitEpisodeComponent(episodePage, episodeState, this);
            }
            */
        }

        private void LoadEpisodeComponents(int _episodePageNum)
        {
            //_currentEpisodeDict.Clear();

            string epilogueNum = "000";

            foreach (var kv in GameManager.EpisodeTitleDict)
            {
                if (kv.Key.Substring(0, 1) == _episodePageNum.ToString() || (_episodePageNum == 1 && kv.Key == epilogueNum))
                {
                    string episodeIndex = kv.Key;
                    string episodeTitle = kv.Value;
                    EpisodeState episodeState = GameManager.Episode.GetEpisodeState(kv.Key);

                    EpisodeComponent e = Instantiate<EpisodeComponent>(_tempEpisodeComponent);
                    e.transform.SetParent(group.transform);
                    e.gameObject.SetActive(true);
                    e.transform.localPosition = _tempEpisodeComponent.transform.localPosition;
                    e.transform.localScale = Vector3.one;

                    e.InitEpisodeComponent(episodeIndex, episodeTitle, episodeState, this);

                    _currentEpisodeDict.Add(kv.Key, e);
                }
            }
        }

        private void ClearEpisodeComponents()
        {
            foreach(var kv in _currentEpisodeDict)
            {
                DestroyImmediate(kv.Value.gameObject);
            }
            _currentEpisodeDict.Clear();
        }

        private void CompareUserPlayData()
        {
            // TODO : 버닝 비버용 강제 에피소드 unlock
            /*
            foreach (var episode in dummyEpisodeList)
            {
                SetEpisodeInteractable(episode);
            }
            */
            /*
            SetEpisodeInteractable(dummyEpisodeList[0]);

            int completeCount = 0;
            foreach (var a in GameManager.UserDataManager.UserPlayData.playedEpisodeDataDict)
            {
                if (a.Value.episodeCompleted)
                    completeCount++;
            }
            for (int i = 0; i < completeCount; i++)
                SetEpisodeInteractable(dummyEpisodeList[i + 1]);
            */
        }
        public void SetEpisodeInteractable(EpisodeComponent episodeComponent)
        {
            //var image = episode.GetComponent<Image>();
            //image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

            AddUIEventWithScrollbar(episodeComponent.gameObject, (a) =>
            {

                GameManager.DummyEpisode = episodeComponent.episodeIndex;
                //GameManager.DummyEpisode = "205"; //테스트 코드
                SceneManager.LoadScene("InGameScene");
            }, Define.UIEvent.Click, scrollRect);
            AddButtonAnim(episodeComponent.gameObject);
        }
        /*
        private void CheckShowingDate()
        {
            if (GameManager.Date.IsDateOpened())
                dateButton.SetActive(true);
        }

        private void CheckShowingDateTutorial()
        {
            if (GameManager.Date.CheckShowingDateTutorial())
            {
                var tutorialPopup = GameManager.UI.ShowPopupUI<UI_DateTutorialPopup>();
                tutorialPopup.InitDateTutorialPopup(dateButton.GetComponent<RectTransform>());

                tutorialPopup.AfterClosePopup += GameManager.Date.CompleteDateTutorial;
            }
        }
        */

        public void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }

        public void OnClickPageClosed(PointerEventData evt)
        {
            GOPageClosed.SetActive(false);
            GOPageOpened.SetActive(true);
        }
        public void OnClickPageOpened(PointerEventData evt)
        {
            GOPageClosed.SetActive(true);
            GOPageOpened.SetActive(false);
        }
    }
}
