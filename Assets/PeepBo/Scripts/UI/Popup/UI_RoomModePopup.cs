using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;

namespace PeepBo.UI.Popup
{
    public class UI_RoomModePopup : UI_Popup
    {
        enum GameObjects
        {
            ExitButton,
            LeftButton,
            RightButton,
        }

        public override void Init()
        {
            base.Init();

            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject exitButton = GetObject((int)GameObjects.ExitButton);
            AddUIEvent(exitButton, OnClickExitButton, Define.UIEvent.Click);
            //AddButtonAnim(exitButton);
        }

        private void OnClickExitButton(PointerEventData evt)
        {
            Debug.Log("Click");
            GameManager.Room.ExitRoomMode();
            // TODO : 탐색 종료 조건 확인하기
        }
    }
}
