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
            // 자동 로그인 시도
            bool autoLoginSuccess = GameManager.Account.TryTokenLogin();

            if (autoLoginSuccess)
                LoginSuccess();
            else // 수동 로그인
                LoginPopup = GameManager.UI.ShowPopupUI<UI_LoginPopup>();
        }

        private bool TryTokenLogin() // 기기 로컬에 저장된 Token으로 자동로그인 시도, Token은 로그인 성공할때마다 자동으로 갱신됨
        {
            BackendReturnObject tokenLoginBRO = Backend.BMember.LoginWithTheBackendToken();

            if (tokenLoginBRO.IsSuccess()) { return true; }
            else
            {
                int statusCode = int.Parse(tokenLoginBRO.GetStatusCode());

                switch (statusCode)
                {
                    case 410: // 1년 지나서 refresh token 만료
                    case 401: // 다른 기기에서 로그인해서 refresh token이 만료됨
                        return false;
                    case 400: // 기기 내에 access token이 존재하지 않음
                        // LoginPopup 띄우기
                        return false;
                }
            }

            return false;
        }

        private void LoginSuccess()
        {
            UpdateUserLoginType(); // 로그인 성공하면 로그인 타입 Update

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

            GameManager.Setting.InitSettingManager(); // 불러온 데이터로 SettingManager 초기화
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
            // GPGS 플러그인 설정
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                .Builder()
                .RequestServerAuthCode(false)
                .RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
                .RequestIdToken()
                .Build();
            //커스텀 된 정보로 GPGS 초기화
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 바꿔주세요.
                                                      //GPGS 시작.
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
            if (Social.localUser.authenticated) // 이미 로그인 된 경우
            {
                BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetGoogleToken(), FederationType.Google, "gpgs");
                if (BRO.IsSuccess())
                {
                    int statusCode = int.Parse(BRO.GetStatusCode());
                    if(statusCode == 200)
                    {
                        LoginSuccess();
                        Debug.Log("구글 로그인 성공");
                    }
                    else if(statusCode == 201)
                    {
                        GameManager.UI.ShowPopupUI<UI_RegitserPolicyPopup>();
                        Debug.Log("구글 회원가입 및 로그인 성공");
                    }
                }
            }
            else
            {
                Social.localUser.Authenticate((bool success) => 
                {
                    if (success)
                    {
                        // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetGoogleToken(), FederationType.Google, "gpgs");
                        if (BRO.IsSuccess())
                        {
                            int statusCode = int.Parse(BRO.GetStatusCode());
                            if (statusCode == 200)
                            {
                                LoginSuccess();
                                Debug.Log("구글 로그인 성공2");
                            }
                            else if (statusCode == 201)
                            {
                                GameManager.UI.ShowPopupUI<UI_RegitserPolicyPopup>();
                                Debug.Log("구글 회원가입 및 로그인 성공2");
                            }
                        }
                    }
                    else
                    {
                        // 로그인 실패
                        Debug.Log("Login failed for some reason");
                    }
                });
            }
        }

        private string GetGoogleToken() // 구글 토큰 받아옴
        {
#if UNITY_ANDROID || UNITY_IOS
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                // 유저 토큰 받기 첫 번째 방법
                string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
                // 두 번째 방법
                // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
                return _IDtoken;
            }
            else
            {
                GameManager.UI.ShowOneButtonPopup("구글 로그인 실패", Application.Quit);
                Debug.Log("접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
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
                if (statusCode == 200) // 기존 유저
                    LoginSuccess();
                else if (statusCode == 201) // 새로운 유저, 회원가입
                    GameManager.UI.ShowPopupUI<UI_RegitserPolicyPopup>();
            }
            else
            {
                if (statusCode == 401)
                {
                    Debug.Log("로컬에 있는 GuestID가 서버에 존재하지 않음 따라서 로컬 GuestID를 삭제하겠음");
                    Backend.BMember.DeleteGuestInfo();

                    GameManager.UI.ShowOneButtonPopup("재시작이 필요합니다", Application.Quit);
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
                        GameManager.UI.ShowOneButtonPopup("구글 계정 연동에 성공하였습니다.");
                        UpdateUserLoginType();
                    }
                    else
                        GameManager.UI.ShowOneButtonPopup("구글 계정 연동에 실패하였습니다.");
                }
                else
                    GameManager.UI.ShowOneButtonPopup("구글 계정 연동에 실패하였습니다.");
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
                Backend.BMember.Logout(); // 뒤끝 로그아웃
                if(userLoginType == "google")
                {
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.SignOut(); // 구글 로그아웃
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
                var bro = Backend.BMember.Logout(); // 뒤끝 로그아웃
                if (bro.IsSuccess())
                {
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.SignOut(); // 구글 로그아웃
                    Debug.Log("구글 로그아웃");
#endif
                }
                else
                    Debug.Log("뒤끝 로그아웃 실패");
            }

            LoginType = AccountLoginType.None;

            SceneManager.LoadScene("LoginScene");
        }

        public void Leave() // 계정 삭제
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