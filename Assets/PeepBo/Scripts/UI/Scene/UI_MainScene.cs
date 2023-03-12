using BackEnd;
using Naninovel;
using PeepBo.Managers;
using PeepBo.UI.Popup;
using PeepBo.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace PeepBo.UI.Scene
{
    public class UI_MainScene : UI_Scene
    {
        enum GameObjects
        {
            StartButton,
            SettingButton,

            Tutorial,
            Check,

            Shop,
            Closet,
            Ad,

            Diary,
            Record,
            Date,

            LogoutButton,
            ChangeButton,
        }

        GameObject startButton, settingButton, checkButton, shopButton, closetButton, adButton, diaryButton, recordButton, dateButton;

        public override void Init()
        {
            base.Init();
            Bind<GameObject>(typeof(GameObjects));
            BindObjects();

            var canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;

            CheckShowingTutorial();
            CheckShowingDate();
        }

        private void BindObjects()
        {
            startButton = GetObject((int)GameObjects.StartButton);
            AddUIEvent(startButton, OnClickStartButton, Define.UIEvent.Click);
            AddButtonAnim(startButton);

            settingButton = GetObject((int)GameObjects.SettingButton);
            AddUIEvent(settingButton, OnClickSettingButton, Define.UIEvent.Click);
            AddButtonAnim(settingButton);

            checkButton = GetObject((int)GameObjects.Check);
            AddUIEvent(checkButton, OnClickCheckButton, Define.UIEvent.Click);
            AddButtonAnim(checkButton);

            shopButton = GetObject((int)GameObjects.Shop);
            AddUIEvent(shopButton, OnClickShopButton, Define.UIEvent.Click);
            AddButtonAnim(shopButton);

            closetButton = GetObject((int) GameObjects.Closet);
            AddUIEvent(closetButton, OnClickClosetButton, Define.UIEvent.Click);
            AddButtonAnim(closetButton);
            
            adButton = GetObject((int) GameObjects.Ad);
            AddUIEvent(adButton, OnClickAdButton, Define.UIEvent.Click);
            AddButtonAnim(adButton);
            
            diaryButton = GetObject((int)GameObjects.Diary);
            AddUIEvent(diaryButton, OnClickDiaryButton, Define.UIEvent.Click);
            AddButtonAnim(diaryButton);

            recordButton = GetObject((int)GameObjects.Record);
            AddUIEvent(recordButton, OnClickRecordButton, Define.UIEvent.Click);
            AddButtonAnim(recordButton);

            dateButton = GetObject((int) GameObjects.Date);
            AddUIEvent(dateButton, OnClickDateButton, Define.UIEvent.Click);
            AddButtonAnim(dateButton);

            GameObject tutorialButton = GetObject((int)GameObjects.Tutorial);
            AddUIEvent(tutorialButton, OnClickTutorialButton, Define.UIEvent.Click);
            AddButtonAnim(tutorialButton);

            GameObject logoutButton = GetObject((int)GameObjects.LogoutButton);
            AddUIEvent(logoutButton, OnClickLogoutButton, Define.UIEvent.Click);

            GameObject changeButton = GetObject((int)GameObjects.ChangeButton);
            AddUIEvent(changeButton, OnClickChangeButton, Define.UIEvent.Click);
        }

        private void OnClickSettingButton(PointerEventData obj)
        {
            GameManager.UI.ShowPopupUI<UI_SettingPopup>();
        }

        private void OnClickDateButton(PointerEventData obj)
        {
            GameManager.UI.ShowPopupUI<UI_DatePopup>();
        }

        private void OnClickChangeButton(PointerEventData obj)
        {
            GameManager.Account.ChangeGuestToFederation();
        }

        private void CheckShowingTutorial()
        {
            if(!GameManager.UserDataManager.UserData.MainTutorialPlayed.Value)
            {
                var tutorialPopup = GameManager.UI.ShowPopupUI<UI_MainTutorialPopup>();

                tutorialPopup.AfterClosePopup += () =>
                {
                    var nicknamePopup = GameManager.UI.ShowPopupUI<UI_RegisterNicknamePopup>();
                    nicknamePopup.AfterClosePopup += () =>
                    {
                        GameManager.UserDataManager.UserData.MainTutorialPlayed = true;
                    };
                };
            }
        }

        private void CheckShowingDate()
        {
            if (GameManager.Date.IsDateOpened())
                dateButton.SetActive(true);
        }

        private void OnClickLogoutButton(PointerEventData evt)
        {
            GameManager.Account.Logout();
        }

        private void OnClickTutorialButton(PointerEventData obj)
        {
            GameManager.UI.ShowPopupUI<UI_MainTutorialPopup>();
        }

        private void OnClickStartButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_EpisodePopup>();
        }

        // todo: 출석체크 popup ui 만들고 수정
        private void OnClickCheckButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_DiaryListPopup>();
        }

        // todo: 상점 popup ui 만들고 수정
        private void OnClickShopButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_RecordPopup>();
        }
        
        // todo: 옷장 popup ui 만들고 수정
        private void OnClickClosetButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_DiaryListPopup>();
        }

        // todo: 일손돕기 popup ui 만들고 수정
        private void OnClickAdButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_RecordPopup>();
        }

        private void OnClickDiaryButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_DiaryListPopup>();
        }

        private void OnClickRecordButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_RecordPopup>();
        }
    }
}