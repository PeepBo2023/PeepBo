using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Managers
{
    public class BackendErrorManager : MonoBehaviour
    {
        void Start()
        {
            // https://developer.thebackend.io/unity3d/guide/error/handler/poll/
            if (Backend.IsInitialized)
            {
                Backend.ErrorHandler.InitializePoll(true);
                Backend.ErrorHandler.OnMaintenanceError = () => {
                    Debug.Log("점검 에러 발생!!!");
                    // 점검 중
                };
                Backend.ErrorHandler.OnTooManyRequestError = () => {
                    Debug.Log("403 에러 발생!!!");
                    // 과도한 요청
                };
                Backend.ErrorHandler.OnTooManyRequestByLocalError = () => {
                    Debug.Log("403 로컬 에러 발생!!!");
                    
                };
                Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
                    Debug.Log("리프레시 불가!!!");
                };
            }
        }

        void Update()
        {
            Backend.ErrorHandler.Poll();
        }
    }
}