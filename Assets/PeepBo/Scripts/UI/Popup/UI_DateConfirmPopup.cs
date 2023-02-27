using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using PeepBo.Managers;

namespace PeepBo.UI.Popup
{
    public class UI_DateConfirmPopup : UI_Popup
    {
        enum GameObjects
        {
            ConfirmButton,
            CloseButton,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private string target;

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, (_) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(closeButton);
        }

        public void InitDateConfirmPopup(string target)
		{
            this.target = target;
        }


        private void OnClickConfirmButton(PointerEventData evt)
        {
            GameManager.DummyEpisode = $"{target}106";
            SceneManager.LoadScene("InGameScene");
            Debug.Log("데이트 진행");
            //ClosePopupUI();
        }
    }
}