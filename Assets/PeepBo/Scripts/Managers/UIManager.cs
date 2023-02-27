using PeepBo.UI.Popup;
using PeepBo.UI.Scene;
using PeepBo.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Managers
{
    public class UIManager
    {
        int popupOrder = 10;
        Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
        UI_Scene sceneUI = null;

        UI_LoadingPopup loadingPopup;

        public UI_Scene CurrentSceneUI { get { return sceneUI; } }

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null)
                    root = new GameObject { name = "@UI_Root" };
                return root;
            }
        }
        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            if (sort)
                canvas.sortingOrder = popupOrder++;
            else
                canvas.sortingOrder = 0;
        }
        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");

            T sceneUI = Util.GetOrAddComponent<T>(go);
            this.sceneUI = sceneUI;
            go.transform.SetParent(Root.transform);

            return sceneUI;
        }

        public T ShowPopupUI<T>(string name = null) where T : UI_Popup
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");

            T popupUI = Util.GetOrAddComponent<T>(go);
            popupStack.Push(popupUI);
            go.transform.SetParent(Root.transform);

            popupUI.OnShowPopup?.Invoke();
            return popupUI;
        }

        public void ClosePopupUI(UI_Popup _popupUI)
        {
            if (popupStack.Count == 0)
                return;
            if (popupStack.Peek() != _popupUI)
            {
                Debug.Log("Close Popup Failed!");
                return;
            }

            UI_Popup popupUI = popupStack.Pop();
            GameManager.Resource.Destroy(popupUI.gameObject);
            popupUI = null;
            popupOrder--;
        }

        public void ClosePopupUI()
        {
            if (popupStack.Count == 0)
                return;
            var topUI = popupStack.Peek();
            if (!topUI.IsEscAble)
                return;

            UI_Popup popupUI = popupStack.Pop();
            popupUI.ClosePopupUIByManager();
            GameManager.Resource.Destroy(popupUI.gameObject);
            popupUI = null;
            popupOrder--;
        }

        public void ShowLoadingPopup()
        {
            loadingPopup = ShowPopupUI<UI_LoadingPopup>();
        }

        public void HideLoadingPopup()
        {
            loadingPopup?.ClosePopupUI();
        }

        public GameObject ShowFadeUI()
        {
            GameObject fadeUI = GameObject.Find("UI_Fade(Clone)");
            if (fadeUI == null)
                fadeUI = GameManager.Resource.Instantiate("UI/Popup/UI_Fade");
            return fadeUI;
        }

        public UI_OneButtonPopup ShowOneButtonPopup(string mainText, Action OnClick = null, Action AfterClosePopup = null)
        {
            var popup = ShowPopupUI<UI_OneButtonPopup>();
            popup.SetMainText(mainText);
            if(OnClick != null)
                popup.OnClick += OnClick;
            if (AfterClosePopup != null)
                popup.AfterClosePopup += AfterClosePopup;
            return popup;
        }

        public UI_TwoButtonPopup ShowTwoButtonPopup(string titleText, string mainText, Action OnConfirm)
        {
            var popup = ShowPopupUI<UI_TwoButtonPopup>();
            popup.SetTitleText(titleText);
            popup.SetMainText(mainText);
            popup.OnConfirm += OnConfirm;
            return popup;
        }
    }
}