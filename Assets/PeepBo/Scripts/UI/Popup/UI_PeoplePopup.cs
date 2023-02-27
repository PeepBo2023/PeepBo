using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;

namespace PeepBo.UI.Popup
{
    public class UI_PeoplePopup : UI_Popup
    {
        public int dummyIdx = 1;
        enum GameObjects
        {
            DummyGroup1,
            DummyGroup2,
            DummyGroup3,
            CloseButton,
            list,
            before,
            after,
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

            GameObject tolist = GetObject((int)GameObjects.list);
            AddUIEvent(tolist, OnClickListButton, Define.UIEvent.Click);
            AddButtonAnim(tolist);

            GameObject tobefore = GetObject((int)GameObjects.before);
            AddUIEvent(tobefore, OnClickBeforeButton, Define.UIEvent.Click);
            AddButtonAnim(tobefore);

            GameObject toafter = GetObject((int)GameObjects.after);
            AddUIEvent(toafter, OnClickAfterButton, Define.UIEvent.Click);
            AddButtonAnim(toafter);
        }

        private void OnClickListButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
        private void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
        private void OnClickBeforeButton(PointerEventData evt)
        {
            GameObject dummys1 = GetObject((int)GameObjects.DummyGroup1);
            GameObject dummys2 = GetObject((int)GameObjects.DummyGroup2);
            GameObject dummys3 = GetObject((int)GameObjects.DummyGroup3);

            dummyIdx--;
            if (dummyIdx <= 0)
            {
                dummyIdx = 3;
            }
            switch (dummyIdx)
            {
                case 1:
                    dummys1.SetActive(true);
                    dummys2.SetActive(false);
                    dummys3.SetActive(false);
                    break;
                case 2:
                    dummys1.SetActive(false);
                    dummys2.SetActive(true);
                    dummys3.SetActive(false);
                    break;
                case 3:
                    dummys1.SetActive(false);
                    dummys2.SetActive(false);
                    dummys3.SetActive(true);
                    break;
            }
        }
        private void OnClickAfterButton(PointerEventData evt)
        {
            GameObject dummys1 = GetObject((int)GameObjects.DummyGroup1);
            GameObject dummys2 = GetObject((int)GameObjects.DummyGroup2);
            GameObject dummys3 = GetObject((int)GameObjects.DummyGroup3);

            dummyIdx++;
            if (dummyIdx >= 4)
            {
                dummyIdx = 1;
            }
            switch (dummyIdx)
            {
                case 1:
                    dummys1.SetActive(true);
                    dummys2.SetActive(false);
                    dummys3.SetActive(false);
                    break;
                case 2:
                    dummys1.SetActive(false);
                    dummys2.SetActive(true);
                    dummys3.SetActive(false);
                    break;
                case 3:
                    dummys1.SetActive(false);
                    dummys2.SetActive(false);
                    dummys3.SetActive(true);
                    break;
            }
        }
    }
}