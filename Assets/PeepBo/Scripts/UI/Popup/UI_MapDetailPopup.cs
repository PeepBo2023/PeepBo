using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;

namespace PeepBo.UI.Popup
{
    public class UI_MapDetailPopup : UI_Popup
    {
        enum GameObjects
        {
            DetailList,
            TitleText
        }

        GameObject detailListObject;
        TextMeshProUGUI titleText;
        List<GameObject> detailList = new List<GameObject>();

		public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            detailListObject = GetObject((int)GameObjects.DetailList);

            GameObject titleTextObject = GetObject((int)GameObjects.TitleText);
            titleText = titleTextObject.GetComponent<TextMeshProUGUI>();

            for(int i=0; i<detailListObject.transform.childCount; i++)
            {
                var detail = detailListObject.transform.GetChild(i).gameObject;
                detailList.Add(detail);
            }

            AddUIEvent(gameObject, (a) => { ClosePopupUI(); }, Define.UIEvent.Click);
        }

        public void SetDetailInfo(string name)
        {
            foreach (var detail in detailList)
            {
                if(detail.name == name)
                {
                    titleText.text = name;
                    detail.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}