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
            Diary,
            Record,
            Date,

            LogoutButton,
            ChangeButton,
        }

        GameObject startButton, settingButton, diaryButton, recordButton, dateButton;

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