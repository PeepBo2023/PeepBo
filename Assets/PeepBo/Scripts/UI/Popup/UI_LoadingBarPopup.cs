using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PeepBo.UI.Popup
{
    public class UI_LoadingBarPopup : UI_Popup
    {
        TextMeshProUGUI mainText, percentText;
        Image loadingBar, gauge;

        int percent = 0;

        enum GameObjects
        {
            MainText,
            Percent,
            LoadingBar,
            Gauge,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();

            IsEscAble = false;
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            mainText = GetObject((int)GameObjects.MainText).GetComponent<TextMeshProUGUI>();
            percentText = GetObject((int)GameObjects.Percent).GetComponent<TextMeshProUGUI>();

            loadingBar = GetObject((int)GameObjects.LoadingBar).GetComponent<Image>();
            gauge = GetObject((int)GameObjects.Gauge).GetComponent<Image>();
        }

        public void InitLoadingBar(string _mainText)
		{
            mainText.text = _mainText;
            percentText.text = "0 %";
		}

        public void UpdateLoadingBar(float _percent)
		{
            percentText.text = $"{(int)_percent} %";
            gauge.fillAmount = _percent / 100.0f;
		}
    }
}