using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using TMPro;
using PeepBo.Managers;
using UnityEngine.SceneManagement;

namespace PeepBo.UI.Popup
{
    public class UI_RegisterNicknamePopup : UI_Popup
    {
        TMP_InputField nicknameText;

        enum GameObjects
        {
            NicknameInputField,
            ConfirmButton,
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

            nicknameText = GetObject((int)GameObjects.NicknameInputField).GetComponent<TMP_InputField>();

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            // TODO : TMP_Validate�̿��ؼ� validate �ϱ�

            if(string.IsNullOrEmpty(nicknameText.text))
            {
                GameManager.UI.ShowOneButtonPopup("�г����� �Է����ּ���");
            }
            else
            {
                // TODO : �г��� �����ؼ� ȸ������ ���� ����
                GameManager.UserDataManager.UserData.Nickname = nicknameText.text;
                ClosePopupUI();
                //GameManager.Login.LoginSuccess();
                //SceneManager.LoadScene("MainScene");
            }
        }
    }
}