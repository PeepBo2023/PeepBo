using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace PeepBo.UI.Popup
{
    public class UI_TwoButtonPopup : UI_Popup
    {
        TextMeshProUGUI mainText, titleText;
        public event Action OnConfirm;

        enum GameObjects
        {
            MainText,
            TitleText,

            ConfirmButton,
            CancelButton,
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

            mainText = GetObject((int)GameObjects.MainText).GetComponent<TextMeshProUGUI>();
            titleText = GetObject((int)GameObjects.TitleText).GetComponent<TextMeshProUGUI>();

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickCancelButton, Define.UIEvent.Click);
            AddButtonAnim(cancelButton);
        }

        public void SetTitleText(string text)
        {
            titleText.text = text;
        }

        public void SetMainText(string text)
        {
            mainText.text = text;
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            OnConfirm?.Invoke();
            ClosePopupUI();
        }

        private void OnClickCancelButton(PointerEventData obj)
        {
            ClosePopupUI();
        }
    }
}