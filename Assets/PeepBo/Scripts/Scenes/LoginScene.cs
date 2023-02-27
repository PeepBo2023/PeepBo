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
                // �ʱ�ȭ ���� �� ����
                Debug.Log("�ʱ�ȭ ����!");
            }
            else
            {
                // �ʱ�ȭ ���� �� ����
                Debug.LogError("�ʱ�ȭ ����!");
                // TODO : ���� ���� ����
            }
        }
    }
}
