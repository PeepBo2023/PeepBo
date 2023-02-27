using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;

namespace PeepBo.UI.Popup
{
    public class UI_UnclePopup : UI_Popup
    {
        public Sprite[] sprites;

        enum Images
        {
            DiaryBackground,
        }
        enum GameObjects
        {
            CloseButton,
            list,
            before,
            after,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();

            sprites = Resources.LoadAll<Sprite>("Sprites/MainScene/¥Ÿ¿ÃæÓ∏Æ/ªÔ√Ã¿« ¿œ±‚");

        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);

            GameObject tolist = GetObject((int)GameObjects.list);
            AddUIEvent(tolist, OnClickListButton, Define.UIEvent.Click);
            AddButtonAnim(tolist);

            /*GameObject tobefore = GetObject((int)GameObjects.before);
            AddUIEvent(tobefore, OnClickBeforeButton, Define.UIEvent.Click);
            AddButtonAnim(tobefore);*/

            /*GameObject toafter = GetObject((int)GameObjects.after);
            AddUIEvent(toafter, OnClickAfterButton, Define.UIEvent.Click);
            AddButtonAnim(toafter);*/

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
            Image background = GetImage((int)Images.DiaryBackground);
            int spriteIdx = sprites.IndexOf(background.sprite);

            if(spriteIdx <= 0)
            {
                spriteIdx = sprites.Length;
            }
            spriteIdx--;
            background.sprite = sprites[spriteIdx];
        }
        private void OnClickAfterButton(PointerEventData evt)
        {
            Image background = GetImage((int)Images.DiaryBackground);
            int spriteIdx = sprites.IndexOf(background.sprite);

            if (spriteIdx >= sprites.Length - 1)
            {
                spriteIdx = -1;
            }
            spriteIdx++;
            background.sprite = sprites[spriteIdx];
        }
    }
}