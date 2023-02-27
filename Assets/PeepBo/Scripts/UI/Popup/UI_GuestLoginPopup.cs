using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace PeepBo.UI.Popup
{
    public class UI_GuestLoginPopup : UI_Popup
    {
        enum GameObjects
        {
            ConfirmButton,
            CancelButton,
        }

        public event Action OnClickConfirm;

        public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickCancelButton, Define.UIEvent.Click);
            AddButtonAnim(cancelButton);
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            ClosePopupUI();
            OnClickConfirm?.Invoke();
            GameManager.Account.TryGuestLogin();
        }

        private void OnClickCancelButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
    }
}