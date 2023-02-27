using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using BackEnd;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PeepBo.Managers;

namespace PeepBo.UI.Popup
{
    public class UI_RegitserPolicyPopup : UI_Popup
    {
        TextMeshProUGUI termText, privacyText;
        Toggle termToggle, privacyToggle;

        enum GameObjects
        {
            TermText,
            TermToggle,

            PrivacyText,
            PrivacyToggle,

            ConfirmButton,
            CancelButton,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();
            InitPolicyText();

            IsEscAble = false;
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            termText = GetObject((int)GameObjects.TermText).GetComponent<TextMeshProUGUI>();
            //privacyText = GetObject((int)GameObjects.PrivacyText).GetComponent<TextMeshProUGUI>();

            termToggle = GetObject((int)GameObjects.TermToggle).GetComponent<Toggle>();
            //privacyToggle = GetObject((int)GameObjects.PrivacyToggle).GetComponent<Toggle>();

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Utils.Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickConfirmButton, Utils.Define.UIEvent.Click);
            AddButtonAnim(cancelButton);
        }

        private void InitPolicyText()
        {
            var bro = Backend.Policy.GetPolicy();
            if (bro.IsSuccess())
            {
                var terms = bro.GetReturnValuetoJSON()["terms"].ToString();
                termText.text = terms;
                /*var privacy = bro.GetReturnValuetoJSON()["privacy"].ToString();
                privacyText.text = privacy;*/
            }
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            if(termToggle.isOn)
            {
                ClosePopupUI();
                GameManager.Account.LoginPopup?.ClosePopupUI();
                GameManager.Account.RegisterSuccess();
            }
            else
                GameManager.UI.ShowOneButtonPopup("모든 약관에 동의하셔야 합니다.");
        }

        /*
        private IEnumerator DownLoadTermText()
        {
            using (var termRequest = UnityWebRequest.Get("https://storage.thebackend.io/59e4216e0d74ecfe26d75441e9940591127d5822a4e1ceeedb16197458a82b9d/terms.html"))
            {
                yield return termRequest.SendWebRequest();

                Debug.Log(termRequest.downloadHandler.text);
            }
        }
        */
    }
}