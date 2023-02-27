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
                    Debug.Log("���� ���� �߻�!!!");
                    // ���� ��
                };
                Backend.ErrorHandler.OnTooManyRequestError = () => {
                    Debug.Log("403 ���� �߻�!!!");
                    // ������ ��û
                };
                Backend.ErrorHandler.OnTooManyRequestByLocalError = () => {
                    Debug.Log("403 ���� ���� �߻�!!!");
                    
                };
                Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
                    Debug.Log("�������� �Ұ�!!!");
                };
            }
        }

        void Update()
        {
            Backend.ErrorHandler.Poll();
        }
    }
}