using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PeepBo.UI.Popup
{
    public class UI_RecordPopup : UI_Popup
    {
        // TODO : 리팩토링, 공모전 위해 임시로 진행
        private int currentSelectIdx = 0;
        public int CurrentSelectIdx 
        {
            get => CurrentSelectIdx;
            private set
            {
                if(value == currentSelectIdx) return;

                selectedImageList[currentSelectIdx].sprite = onDeSelectSpriteList[currentSelectIdx]; // 원래 select 되어 있던 이미지 deselect

                dateBackgroundImage.sprite = dateBackgroundList[value];

                selectedImageList[value].sprite = onSelectSpriteList[value]; // 원래 deselect 되어 있던 이미지 select

                currentSelectIdx = value; // idx update
            }
        }

        [SerializeField] List<Sprite> dateBackgroundList = new List<Sprite>();
        [SerializeField] List<Sprite> onSelectSpriteList = new List<Sprite>();
        [SerializeField] List<Sprite> onDeSelectSpriteList = new List<Sprite>();

        Image dateBackgroundImage;
        List<Image> selectedImageList = new List<Image>();


        enum GameObjects
        {
            CloseButton,
            DateBackground,
            SelectList,
            고결,
            우준,
            유겸,
            다함,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            dateBackgroundImage = GetObject((int)GameObjects.DateBackground).GetComponent<Image>();

            GameObject selectListObject = GetObject((int)GameObjects.SelectList);
            for (int i = 0; i < selectListObject.transform.childCount; i++)
            {
                int capture = i;
                var child = selectListObject.transform.GetChild(i);
                selectedImageList.Add(child.GetComponent<Image>());
                AddUIEvent(child.gameObject, (_) => { CurrentSelectIdx = capture; }, Define.UIEvent.Click);
            }


            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);
        }

        private void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
    }
}