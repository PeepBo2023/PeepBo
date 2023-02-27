using Naninovel;
using PeepBo.Managers;
using PeepBo.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BackEnd;
using TMPro;
using PeepBo.UI.Popup;

namespace PeepBo.UI.Scene
{
    public class UI_LoginScene : UI_Scene
    {
        enum GameObjects
        {
            TouchToStart,
            MainBackground,
            DownloadBackground,
        }

        GameObject startButton;
        GameObject mainBackground, downloadBackground;
        

        public override void Init()
        {
            base.Init();
            Bind<GameObject>(typeof(GameObjects));
            BindObjects();

            GameManager.Account.LoginScene = this;
        }

        public void AfterLogin()
        {
            mainBackground?.SetActive(false);
            downloadBackground?.SetActive(true);
        }

        
        private void BindObjects()
        {
            startButton = GetObject((int)GameObjects.TouchToStart);
            AddUIEvent(startButton, OnClickStartButton, Define.UIEvent.Click);

            mainBackground = GetObject((int)GameObjects.MainBackground);
            downloadBackground = GetObject((int)GameObjects.DownloadBackground);
        }

        private void OnClickStartButton(PointerEventData evt)
		{
            var script = startButton.GetComponent<TouchToStart>();
            script.OnClick();
            GameManager.Account.TryLogin();
		}
    }
}