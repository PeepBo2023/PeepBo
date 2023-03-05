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

namespace PeepBo.UI.Popup
{
    public class UI_EpisodePopup : UI_Popup
    {
        enum GameObjects
        {
            CloseButton,
            DummyGroup,

            ScrollRect,

            LockedImage,
            PlayingImage,
            NewImage,

            //Diary,
            //Date,
        }

        GameObject dummyGroup;
        GameObject dateButton;
        List<GameObject> dummyEpisodeList = new List<GameObject> ();
        ScrollRect scrollRect;

        public Image lockedImage, playingImage, newImage;

        public override void Init()
        {
            base.Init();
            BindObjects();

            //CheckShowingDate();

            InitEpisodeComponents();

            CompareUserPlayData();

            OnShowPopup += CheckShowingDateTutorial;
        }

        private void BindObjects()
        {
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
            /*
            dateButton = GetObject((int)GameObjects.Date);
            AddUIEvent(dateButton, (_) => { GameManager.UI.ShowPopupUI<UI_DatePopup>(); }, Define.UIEvent.Click);
            AddButtonAnim(dateButton);
            */
        }

        private void InitEpisodeComponents()
        {
            foreach(var episode in dummyEpisodeList)
            {
                string identifier = episode.name.Substring(5, 3);
                var episodeComponet = episode.GetComponent<EpisodeComponent>();

                var episodeState = GameManager.Episode.GetEpisodeState(identifier);
                episodeComponet.InitEpisodeComponent(episodeState, this);
            }
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
        public void SetEpisodeInteractable(GameObject episode)
        {
            //var image = episode.GetComponent<Image>();
            //image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

            AddUIEventWithScrollbar(episode, (a) =>
            {
                
                GameManager.DummyEpisode = episode.name.Substring(5);
                //GameManager.DummyEpisode = "205"; //테스트 코드
                SceneManager.LoadScene("InGameScene");
            }, Define.UIEvent.Click, scrollRect);
            AddButtonAnim(episode);
        }

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

        private void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
    }
}
