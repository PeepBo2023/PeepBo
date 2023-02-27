using UnityEngine;
using PeepBo.Managers;
using PeepBo.UI.Scene;
using BackEnd;
using PeepBo.UI.Popup;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace PeepBo.Scene
{ 
    public class LoginScene : BaseScene
    {
        [SerializeField] AudioSource bgmSource;

        private void Start()
                => Init();

        protected override void Init()
        {
            base.Init();

            if(!Backend.IsInitialized)
                BackEndInitialize();

            GameManager.Init();

            GameManager.Audio.SetLoginSceneBGM(bgmSource);
            GameManager.UI.ShowSceneUI<UI_LoginScene>();
        }


        private void BackEndInitialize()
        {
            var bro = Backend.Initialize(true);
            if (bro.IsSuccess())
            {
                // 초기화 성공 시 로직
                Debug.Log("초기화 성공!");
            }
            else
            {
                // 초기화 실패 시 로직
                Debug.LogError("초기화 실패!");
                // TODO : 어플 강제 종료
            }
        }
    }
}
