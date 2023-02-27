using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;

namespace PeepBo.UI.Popup
{
    public class UI_DiaryListPopup : UI_Popup
    {
        enum GameObjects
        {
            CloseButton,
            Dummy1,
            Dummy2,
            Dummy3,
            Dummy4,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);

            GameObject dummy1 = GetObject((int)GameObjects.Dummy1);
            AddUIEvent(dummy1, OnClickHaerokButton, Define.UIEvent.Click);
            AddButtonAnim(dummy1);

            GameObject dummy2 = GetObject((int)GameObjects.Dummy2);
            AddUIEvent(dummy2, OnClickMapButton, Define.UIEvent.Click);
            AddButtonAnim(dummy2);

            GameObject dummy3 = GetObject((int)GameObjects.Dummy3);
            AddUIEvent(dummy3, OnClickPeopleButton, Define.UIEvent.Click);
            AddButtonAnim(dummy3);

            GameObject dummy4 = GetObject((int)GameObjects.Dummy4);
            AddUIEvent(dummy4, OnClickUncleButton, Define.UIEvent.Click);
            AddButtonAnim(dummy4);
        }

        private void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
        private void OnClickHaerokButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_HaerokdoPopup>();
        }
        private void OnClickMapButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_MapPopup>();
        }
        private void OnClickPeopleButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_PeoplePopup>();
        }
        private void OnClickUncleButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_UnclePopup>();
        }
    }
}