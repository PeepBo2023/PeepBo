using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PeepBo.UI.Popup;
using PeepBo.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeepBo.Managers
{
    public class AccountManager
    {
        public UI_LoginScene LoginScene { get; set; }
        public UI_LoginPopup LoginPopup { get; set; }

        public enum AccountLoginType
        {
            None,

            Guest,
            Google,
            Apple
        }

        public AccountLoginType LoginType { get; private set; } = AccountLoginType.None;

        public event Action<AccountLoginType> OnUpdateLoginType;

        public void TryLogin()
        {
            // �ڵ� �α��� �õ�
            bool autoLoginSuccess = GameManager.Account.TryTokenLogin();

            if (autoLoginSuccess)
                LoginSuccess();
            else // ���� �α���
                LoginPopup = GameManager.UI.ShowPopupUI<UI_LoginPopup>();
        }

        private bool TryTokenLogin() // ��� ���ÿ� ����� Token���� �ڵ��α��� �õ�, Token�� �α��� �����Ҷ����� �ڵ����� ���ŵ�
        {
            BackendReturnObject tokenLoginBRO = Backend.BMember.LoginWithTheBackendToken();

            if (tokenLoginBRO.IsSuccess()) { return true; }
            else
            {
                int statusCode = int.Parse(tokenLoginBRO.GetStatusCode());

                switch (statusCode)
                {
                    case 410: // 1�� ������ refresh token ����
                    case 401: // �ٸ� ��⿡�� �α����ؼ� refresh token�� �����
                        return false;
                    case 400: // ��� ���� access token�� �������� ����
                        // LoginPopup ����
                        return false;
                }
            }

            return false;
        }

        private void LoginSuccess()
        {
            UpdateUserLoginType(); // �α��� �����ϸ� �α��� Ÿ�� Update

            LoginPopup?.ClosePopupUI();
            LoginScene?.AfterLogin();
            GameManager.Update.Add(LoadData());
        }

        public void RegisterSuccess()
        {
            LoginSuccess();
        }

        private IEnumerator LoadData()
        {
            var downloadData = GameManager.Data.TryDownloadData();
            while (downloadData.MoveNext())
                yield return null;

            GameManager.UserDataManager.TryLoadUserData();

            GameManager.Setting.InitSettingManager(); // �ҷ��� �����ͷ� SettingManager �ʱ�ȭ
            /*
            var loadUserData = GameManager.UserDataManager.TryLoadUserData();
            while (loadUserData.MoveNext())
                yield return null;
            */

            SceneManager.LoadScene("MainScene");
        }

        private void SetGPGSConfiguration()
        {
#if UNITY_ANDROID
            // GPGS �÷����� ����
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                .Builder()
                .RequestServerAuthCode(false)
                .RequestEmail() // �̸��� ������ ��� ���� �ʴٸ� �ش� ��(RequestEmail)�� �����ּ���.
                .RequestIdToken()
                .Build();
            //Ŀ���� �� ������ GPGS �ʱ�ȭ
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true; // ����� �α׸� ���� ���� �ʴٸ� false�� �ٲ��ּ���.
                                                      //GPGS ����.
            PlayGamesPlatform.Activate();
#endif
        }

        public void TryGoogleLogin()
        {
            SetGPGSConfiguration();
            GPGSLogin();
        }

        private void GPGSLogin()
        {
            if (Social.localUser.authenticated) // �̹� �α��� �� ���
            {
                BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetGoogleToken(), FederationType.Google, "gpgs");
                if (BRO.IsSuccess())
                {
                    int statusCode = int.Parse(BRO.GetStatusCode());
                    if(statusCode == 200)
                    {
                        LoginSuccess();
                        Debug.Log("���� �α��� ����");
                    }
                    else if(statusCode == 201)
                    {
                        GameManager.UI.ShowPopupUI<UI_RegitserPolicyPopup>();
                        Debug.Log("���� ȸ������ �� �α��� ����");
                    }
                }
            }
            else
            {
                Social.localUser.Authenticate((bool success) => 
                {
                    if (success)
                    {
                        // �α��� ���� -> �ڳ� ������ ȹ���� ���� ��ū���� ���� ��û
                        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetGoogleToken(), FederationType.Google, "gpgs");
                        if (BRO.IsSuccess())
                        {
                            int statusCode = int.Parse(BRO.GetStatusCode());
                            if (statusCode == 200)
                            {
                                LoginSuccess();
                                Debug.Log("���� �α��� ����2");
                            }
                            else if (statusCode == 201)
                            {
                                GameManager.UI.ShowPopupUI<UI_RegitserPolicyPopup>();
                                Debug.Log("���� ȸ������ �� �α��� ����2");
                            }
                        }
                    }
                    else
                    {
                        // �α��� ����
                        Debug.Log("Login failed for some reason");
                    }
                });
            }
        }

        private string GetGoogleToken() // ���� ��ū �޾ƿ�
        {
#if UNITY_ANDROID || UNITY_IOS
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                // ���� ��ū �ޱ� ù ��° ���
                string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
                // �� ��° ���
                // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
                return _IDtoken;
            }
            else
            {
                GameManager.UI.ShowOneButtonPopup("���� �α��� ����", Application.Quit);
                Debug.Log("���ӵǾ� ���� �ʽ��ϴ�. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
                return null;
            }
#else
            return null;
#endif
        }

        public void TryGuestLogin()
        {
            //var loadingPopup = GameManager.UI.ShowPopupUI<UI_LoadingPopup>();

            var bro = Backend.BMember.GuestLogin();
            int statusCode = int.Parse(bro.GetStatusCode());
            if (bro.IsSuccess())
            {
                if (statusCode == 200) // ���� ����
                    LoginSuccess();
                else if (statusCode == 201) // ���ο� ����, ȸ������
                    GameManager.UI.ShowPopupUI<UI_RegitserPolicyPopup>();
            }
            else
            {
                if (statusCode == 401)
                {
                    Debug.Log("���ÿ� �ִ� GuestID�� ������ �������� ���� ���� ���� GuestID�� �����ϰ���");
                    Backend.BMember.DeleteGuestInfo();

                    GameManager.UI.ShowOneButtonPopup("������� �ʿ��մϴ�", Application.Quit);
                }
            }
        }

        public void ChangeGuestToFederation()
        {
            SetGPGSConfiguration();
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    BackendReturnObject bro = Backend.BMember.ChangeCustomToFederation(GetGoogleToken(), FederationType.Google);
                    if (bro.IsSuccess())
                    {
                        GameManager.UI.ShowOneButtonPopup("���� ���� ������ �����Ͽ����ϴ�.");
                        UpdateUserLoginType();
                    }
                    else
                        GameManager.UI.ShowOneButtonPopup("���� ���� ������ �����Ͽ����ϴ�.");
                }
                else
                    GameManager.UI.ShowOneButtonPopup("���� ���� ������ �����Ͽ����ϴ�.");
            });
        }

        private void UpdateUserLoginType()
        {
            BackendReturnObject bro = Backend.BMember.GetUserInfo();
            if (bro.IsSuccess())
            {
                string userLoginType = bro.GetReturnValuetoJSON()["row"]["subscriptionType"].ToString();
                Debug.Log(userLoginType);
                var prevLoginType = LoginType;

                Debug.Log(prevLoginType);
                if (userLoginType == "customSignUp")
                    LoginType = AccountLoginType.Guest;
                else if (userLoginType == "google")
                    LoginType = AccountLoginType.Google;
                else if (userLoginType == "apple")
                    LoginType = AccountLoginType.Apple;

                if(prevLoginType != AccountLoginType.None)
                    OnUpdateLoginType?.Invoke(LoginType);
            }
        }

        public void Logout()
        {
            /*
            BackendReturnObject bro = Backend.BMember.GetUserInfo();
            if (bro.IsSuccess())
            {
                string userLoginType = bro.GetReturnValuetoJSON()["row"]["subscriptionType"].ToString();
                Backend.BMember.Logout(); // �ڳ� �α׾ƿ�
                if(userLoginType == "google")
                {
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.SignOut(); // ���� �α׾ƿ�
#endif
                }
                else if(userLoginType == "apple")
				{

				}
            }
            */

            if (LoginType == AccountLoginType.Guest)
                Leave();
            else if (LoginType == AccountLoginType.Google)
            {
                var bro = Backend.BMember.Logout(); // �ڳ� �α׾ƿ�
                if (bro.IsSuccess())
                {
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.SignOut(); // ���� �α׾ƿ�
                    Debug.Log("���� �α׾ƿ�");
#endif
                }
                else
                    Debug.Log("�ڳ� �α׾ƿ� ����");
            }

            LoginType = AccountLoginType.None;

            SceneManager.LoadScene("LoginScene");
        }

        public void Leave() // ���� ����
        {
            if(LoginType == AccountLoginType.Guest)
            {
                Backend.BMember.DeleteGuestInfo();
                Backend.BMember.WithdrawAccount();
            }
            else
            {
                Backend.BMember.WithdrawAccount();
            }

            LoginType = AccountLoginType.None;

            SceneManager.LoadScene("LoginScene");
        }
    }
}