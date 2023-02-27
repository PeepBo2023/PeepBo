using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace PeepBo.UI.Popup
{
    public class UI_DatePopup : UI_Popup
    {
        enum GameObjects
        {
            CloseButton,
            ���,
            ����,
            ����,
            ����,
        }

        GameObject ���, ����, ����, ����;

        public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            ��� = GetObject((int)GameObjects.���);
            AddUIEvent(���, (_)=>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("G");
            },Define.UIEvent.Click);
            AddButtonAnim(���);
            /*
            ���� = GetObject((int)GameObjects.����);
            AddUIEvent(����, (_) =>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("W");
            }, Define.UIEvent.Click);
            AddButtonAnim(����);

            ���� = GetObject((int)GameObjects.����);
            AddUIEvent(����, (_) =>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("Y");
            }, Define.UIEvent.Click);
            AddButtonAnim(����);
            */

            ���� = GetObject((int)GameObjects.����);
            AddUIEvent(����, (_) =>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("D");
            }, Define.UIEvent.Click);
            AddButtonAnim(����);

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, (_) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(closeButton);
        }
	}
}