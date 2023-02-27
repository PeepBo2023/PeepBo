using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using BackEnd;

namespace PeepBo.UI.Popup
{
    public class UI_LoginPopup : UI_Popup
    {
        enum GameObjects
        {
            GuestLoginButton,
            GoogleLoginButton,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();

            IsEscAble = false;
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject guestLoginButton = GetObject((int)GameObjects.GuestLoginButton);
            AddUIEvent(guestLoginButton, OnClickGuestLoginButton, Define.UIEvent.Click);
            AddButtonAnim(guestLoginButton);

            GameObject googleLoginButton = GetObject((int)GameObjects.GoogleLoginButton);
            AddUIEvent(googleLoginButton, OnClickGoogleLoginButton, Define.UIEvent.Click);
            AddButtonAnim(googleLoginButton);
        }

        private void OnClickGuestLoginButton(PointerEventData evt)
        {
            var guestLoginPopup = GameManager.UI.ShowPopupUI<UI_GuestLoginPopup>();
            guestLoginPopup.OnClickConfirm += () => { GameManager.UI.ClosePopupUI(this); };
        }

        private void OnClickGoogleLoginButton(PointerEventData evt)
        {
            GameManager.Account.TryGoogleLogin();
        }
    }
}