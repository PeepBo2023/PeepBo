using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace PeepBo.UI.Popup
{
    public class UI_OneButtonPopup : UI_Popup
    {
        TextMeshProUGUI mainText;
        public event Action OnClick;

        enum GameObjects
        {
            MainText,
            ConfirmButton,
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

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }

        public void SetMainText(string text)
        {
            mainText.text = text;
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            OnClick?.Invoke();
            ClosePopupUI();
        }
    }
}