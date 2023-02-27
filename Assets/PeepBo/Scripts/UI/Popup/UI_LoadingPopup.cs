using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace PeepBo.UI.Popup
{
    public class UI_LoadingPopup : UI_Popup
    {
        enum GameObjects
        {
            Circle,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();

            StartCoroutine(Loading());

            IsEscAble = false;
        }

        List<GameObject> iconList = new List<GameObject>();

        int currentIndex = 0;

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject circle = GetObject((int)GameObjects.Circle);

            for(int i=0; i<circle.transform.childCount; i++)
                iconList.Add(circle.transform.GetChild(i).gameObject);

        }

        private IEnumerator Loading()
        {
            var cached = new WaitForSeconds(0.25f);
            while (true)
            {
                iconList[currentIndex].SetActive(true);
                
                yield return cached;

                iconList[currentIndex].SetActive(false);

                currentIndex++;
                if (currentIndex > 7)
                    currentIndex = 0;
            }
        } 

        public void OnLoadComplete()
        {
            ClosePopupUI();
        }
    }
}