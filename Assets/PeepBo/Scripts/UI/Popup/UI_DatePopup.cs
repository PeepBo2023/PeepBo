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
            고결,
            우준,
            유겸,
            다함,
        }

        GameObject 고결, 우준, 유겸, 다함;

        public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            고결 = GetObject((int)GameObjects.고결);
            AddUIEvent(고결, (_)=>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("G");
            },Define.UIEvent.Click);
            AddButtonAnim(고결);
            /*
            우준 = GetObject((int)GameObjects.우준);
            AddUIEvent(우준, (_) =>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("W");
            }, Define.UIEvent.Click);
            AddButtonAnim(우준);

            유겸 = GetObject((int)GameObjects.유겸);
            AddUIEvent(유겸, (_) =>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("Y");
            }, Define.UIEvent.Click);
            AddButtonAnim(유겸);
            */

            다함 = GetObject((int)GameObjects.다함);
            AddUIEvent(다함, (_) =>
            {
                var popup = GameManager.UI.ShowPopupUI<UI_DateConfirmPopup>();
                popup.InitDateConfirmPopup("D");
            }, Define.UIEvent.Click);
            AddButtonAnim(다함);

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, (_) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(closeButton);
        }
	}
}