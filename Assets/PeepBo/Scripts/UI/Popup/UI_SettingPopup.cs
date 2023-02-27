using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace PeepBo.UI.Popup
{
    public class UI_SettingPopup : UI_Popup
    {
        enum GameObjects
        {
            CloseButton,
            
            ButtonList,

            MainButton,
            AccountButton,
            CustomerButton,

            PanelList,

            MainPanel,

            BGMSlider,
            SFXSlider,
            VoiceSlider,

            AccountPanel,

            NameChangeButton,
            NameInputField,
            GoogleAccountButton,
            AppleAccountButton,
            GuestAccountButton,
            ResetButton,
            LogoutButton,
            LeaveButton,

            CustomerPanel,
        }

        enum Images
        {
            OnOnImage,
            OnOffImage,
            OffOnImage,
            OffOffImage,

            VibrationOn,
            VibrationOff,
            NotificationOn,
            NotificationOff,
        }

        enum TMPs
        {
            GuestAccountText,
            GoogleAccountText,
            AppleAccountText,
        }

        int currentSelectIndex = 0;
        bool isVibrationOn, isNotificationOn;

        List<GameObject> buttonList = new List<GameObject>();
        List<GameObject> pannelList = new List<GameObject>();
        Image onOn, onOff, offOn, offOff;

        Slider bgmSlider, sfxSlider, voiceSlider;
        Image vibrationOn, vibrationOff, notificationOn, notificationOff;
        TMP_InputField nameInputField;

        TextMeshProUGUI googleAccountText, appleAccountText, guestAccountText;
        GameObject googleAccountButton, appleAccountButton, guestAccountButton;


        public override void Init()
        {
            base.Init();

            BindObjects();

            InitSettingPopup();
        }

        private void InitSettingPopup()
        {
            var settingData = GameManager.Setting.SettingData;
            var userData = GameManager.Setting.UserData;

            InitMainPanel();
            InitAccountPanel();

            void InitMainPanel()
            {
                bgmSlider.value = settingData.bgmSound;
                bgmSlider.onValueChanged.AddListener((volume)=>GameManager.Audio.UpdateBGMVolume(volume));
                sfxSlider.value = settingData.sfxSound;
                sfxSlider.onValueChanged.AddListener((volume) => GameManager.Audio.UpdateSFXVolume(volume));
                voiceSlider.value = settingData.voiceSound;

                vibrationOn.sprite = settingData.vibrationOn ? onOn.sprite : onOff.sprite;
                vibrationOff.sprite = !settingData.vibrationOn ? offOn.sprite : offOff.sprite;
                notificationOn.sprite = settingData.notificationOn ? onOn.sprite : onOff.sprite;
                notificationOff.sprite = !settingData.notificationOn ? offOn.sprite : offOff.sprite;

                isVibrationOn = settingData.vibrationOn;
                isNotificationOn = settingData.notificationOn;
            }

            void InitAccountPanel()
            {
                nameInputField.text = userData.Nickname;

                OnUpdateLoginType(GameManager.Account.LoginType);
                GameManager.Account.OnUpdateLoginType += OnUpdateLoginType;
            }
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(TMPs));

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);

            Transform buttonListTransform = GetObject((int)GameObjects.ButtonList).transform;
            for(int i=0; i<buttonListTransform.childCount; i++)
            {
                int lambda = i;
                var button = buttonListTransform.GetChild(i).gameObject;
                AddUIEvent(button, (_) => PanelSwitch(lambda), Define.UIEvent.Click);
                buttonList.Add(button);
            }

            Transform pannelListTransform = GetObject((int)GameObjects.PanelList).transform;
            for (int i = 0; i < pannelListTransform.childCount; i++)
            {
                var panel = pannelListTransform.GetChild(i).gameObject;
                pannelList.Add(panel);
            }

            BindMainPanel();
            BindAccountPanel();

            /*
            GameObject guestLoginButton = GetObject((int)GameObjects.GuestLoginButton);
            AddUIEvent(guestLoginButton, OnClickGuestLoginButton, Define.UIEvent.Click);
            AddButtonAnim(guestLoginButton);

            GameObject googleLoginButton = GetObject((int)GameObjects.GoogleLoginButton);
            AddUIEvent(googleLoginButton, OnClickGoogleLoginButton, Define.UIEvent.Click);
            AddButtonAnim(googleLoginButton);
            */
        }

        private void BindMainPanel()
        {
            onOn = GetImage((int)Images.OnOnImage);
            onOff = GetImage((int)Images.OnOffImage);
            offOn = GetImage((int)Images.OffOnImage);
            offOff = GetImage((int)Images.OffOffImage);

            bgmSlider = GetObject((int)GameObjects.BGMSlider).GetComponent<Slider>();
            sfxSlider = GetObject((int)GameObjects.SFXSlider).GetComponent<Slider>();
            voiceSlider = GetObject((int)GameObjects.VoiceSlider).GetComponent<Slider>();

            vibrationOn = GetImage((int)Images.VibrationOn);
            vibrationOff = GetImage((int)Images.VibrationOff);
            notificationOn = GetImage((int)Images.NotificationOn);
            notificationOff = GetImage((int)Images.NotificationOff);

            AddUIEvent(vibrationOn.gameObject, (_) => UpdateVibration(true), Define.UIEvent.Click);
            AddUIEvent(vibrationOff.gameObject, (_) => UpdateVibration(false), Define.UIEvent.Click);
            AddUIEvent(notificationOn.gameObject, (_) => UpdateNotification(true), Define.UIEvent.Click);
            AddUIEvent(notificationOff.gameObject, (_) => UpdateNotification(false), Define.UIEvent.Click);

            void UpdateVibration(bool value)
            {
                if (isVibrationOn == value) return;

                vibrationOn.sprite = value ? onOn.sprite : onOff.sprite;
                vibrationOff.sprite = !value ? offOn.sprite : offOff.sprite;

                isVibrationOn = value;
            }
            void UpdateNotification(bool value)
            {
                if (isNotificationOn == value) return;

                notificationOn.sprite = value ? onOn.sprite : onOff.sprite;
                notificationOff.sprite = !value ? offOn.sprite : offOff.sprite;

                isNotificationOn = value;
            }
        }

        private void BindAccountPanel()
        {
            GameObject nameChangeButton = GetObject((int)GameObjects.NameChangeButton);
            googleAccountButton = GetObject((int)GameObjects.GoogleAccountButton);
            appleAccountButton = GetObject((int)GameObjects.AppleAccountButton);
            guestAccountButton = GetObject((int)GameObjects.GuestAccountButton);
            GameObject resetButton = GetObject((int)GameObjects.ResetButton);
            GameObject logoutButton = GetObject((int)GameObjects.LogoutButton);
            GameObject leaveButton = GetObject((int)GameObjects.LeaveButton);
            nameInputField = GetObject((int)GameObjects.NameInputField).GetComponent<TMP_InputField>();
            guestAccountText = GetTextPro((int)TMPs.GuestAccountText);
            googleAccountText = GetTextPro((int)TMPs.GoogleAccountText);
            appleAccountText = GetTextPro((int)TMPs.AppleAccountText);

            AddUIEvent(nameChangeButton, OnClickNameChangeButton, Define.UIEvent.Click);
            if(GameManager.Account.LoginType != AccountManager.AccountLoginType.Google)
                AddUIEvent(googleAccountButton, OnClickGoogleAccountButton, Define.UIEvent.Click);
            AddUIEvent(appleAccountButton, OnClickAppleAccountButton, Define.UIEvent.Click);
            AddUIEvent(guestAccountButton, OnClickGuestAccountButton, Define.UIEvent.Click);
            AddUIEvent(resetButton, OnClickResetButton, Define.UIEvent.Click);
            AddUIEvent(logoutButton, OnClickLogoutButton, Define.UIEvent.Click);
            AddUIEvent(leaveButton, OnClickLeaveButton, Define.UIEvent.Click);

            AddButtonAnim(nameChangeButton);

        }

        private void OnClickLeaveButton(PointerEventData obj)
        {
            var popup = GameManager.UI.ShowPopupUI<UI_TwoButtonPopup>();
            popup.SetTitleText("���� ����");
            popup.SetMainText("������ ������ ��� ��� �����Ϳ� ���� ������\n�����Ǹ� ������ �� �����ϴ�.\n<b>����</b>�Ͻðڽ��ϱ� ?");
            popup.OnConfirm += () => GameManager.Account.Leave();
        }

        private void OnClickLogoutButton(PointerEventData obj)
        {
            var popup = GameManager.UI.ShowPopupUI<UI_TwoButtonPopup>();
            popup.SetTitleText("�α׾ƿ�");
            popup.SetMainText("�α׾ƿ� �Ͻðڽ��ϱ�?\n(�Խ�Ʈ ������ ��� ������ �����˴ϴ�.)");
            popup.OnConfirm += () => GameManager.Account.Logout();
        }

        private void OnClickResetButton(PointerEventData obj)
        {
            throw new NotImplementedException();
        }

        private void OnClickGuestAccountButton(PointerEventData obj)
        {
            //
        }

        private void OnClickAppleAccountButton(PointerEventData obj)
        {
            throw new NotImplementedException();
        }

        private void OnClickGoogleAccountButton(PointerEventData obj)
        {
            GameManager.Account.ChangeGuestToFederation();
        }

        private void OnClickNameChangeButton(PointerEventData obj)
        {
            var newName = nameInputField.text;
            if (GameManager.Setting.UserData.Nickname == newName)
            {
                GameManager.UI.ShowOneButtonPopup("������ �г����Դϴ�.");
                return; 
            }

            GameManager.Setting.TryUpdateName(newName,
                () =>
                {
                    GameManager.UI.ShowOneButtonPopup("�г��� ������ �Ϸ�Ǿ����ϴ�.");
                },
                () =>
                {
                    GameManager.UI.ShowOneButtonPopup("�г����� ��ȿ���� �ʽ��ϴ�.");
                    newName = GameManager.Setting.UserData.Nickname;
                });
        }

        private void PanelSwitch(int index)
        {
            if (index == currentSelectIndex) 
                return;

            DeActivate(currentSelectIndex);
            Activate(index);

            currentSelectIndex = index;

            void DeActivate(int index)
            {
                var target = buttonList[index];
                var image = target.GetComponentInChildren<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
                var text = target.GetComponentInChildren<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.4f);

                pannelList[index].SetActive(false);
            }

            void Activate(int index)
            {
                var target = buttonList[index];
                var image = target.GetComponentInChildren<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                var text = target.GetComponentInChildren<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

                pannelList[index].SetActive(true);
            }
        }

        private void OnUpdateLoginType(AccountManager.AccountLoginType newType)
        {
            if (newType == AccountManager.AccountLoginType.Guest)
            {
                googleAccountText.text = "Connect with google";
                guestAccountText.text = "�Խ�Ʈ�� �α��ε�";
            }
            else if (newType == AccountManager.AccountLoginType.Google)
            {
                RemoveUIEvent(googleAccountButton, OnClickGoogleAccountButton, Define.UIEvent.Click); ;
                googleAccountText.text = "Connected with google";
                guestAccountText.text = "���۷� �α��ε�";
            }
            else if (newType == AccountManager.AccountLoginType.Apple)
            {

            }
        }

        private void OnClickCloseButton(PointerEventData obj)
        {
            GameManager.Setting.SaveSetting(bgmSlider.value, sfxSlider.value, voiceSlider.value, isVibrationOn, isNotificationOn);
            ClosePopupUI();
        }
    }
}