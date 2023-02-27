using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PeepBo.UI.Popup
{
    public class UI_MapPopup : UI_Popup
    {
        enum GameObjects
        {
            CloseButton,
            list,
            DetailList,
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

            GameObject detailList = GetObject((int) GameObjects.DetailList);

            for(int i=0; i<detailList.transform.childCount; i++)
            {
                var detail = detailList.transform.GetChild(i).gameObject;
                AddUIEvent(detail, (_) => 
                {
                    var popup = GameManager.UI.ShowPopupUI<UI_MapDetailPopup>();
                    popup.SetDetailInfo(detail.name);
                }, Define.UIEvent.Click);
                AddButtonAnim(detail);
            }
        }

        private void OnClickListButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
        private void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
    }
}