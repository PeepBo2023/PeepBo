using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shaker : MonoBehaviour
{
    [SerializeField] private int targetCount;
    [SerializeField] private Image fillImage;

    int currentCount = 0;
    bool isShow = false;

    public float shakeThreshold = 1.0f; // 흔들기를 인식할 가속도 임계값
    public float updateInterval = 0.5f; // 흔들기 검사를 수행하는 간격

    private float lastUpdateTime = 0.0f;
    private Vector3 lastAcceleration;



    private void Start()
    {
        fillImage.fillAmount = (float)currentCount / targetCount;
        var customUI = GetComponent<CustomUI>();
        customUI.OnVisibilityChanged += CustomUI_OnVisibilityChanged;

        lastAcceleration = Input.acceleration;
    }

    private void CustomUI_OnVisibilityChanged(bool _isShow)
    {
        if (_isShow)
            OnShow();
        else
            OnHide();
    }

    private void OnShow()
    {
        if (isShow) return;
        isShow = true;
    }

    private void OnHide()
    {
        isShow = false;
    }

    private void Update()
    {
        //테스트를 위해서 일단 버튼 클릭하는 기능을 살려둔다.
        if (Input.GetMouseButtonDown(0) && isShow)
        {

            bool bResult = AddCount(1);

            if(bResult == false)
            {
                Close();
            }

        };


        // 일정 시간마다 흔들기 검사를 수행
        if (Time.time - lastUpdateTime > updateInterval)
        {
            Vector3 deltaAcceleration = Input.acceleration - lastAcceleration;  // 가속도 변화량을 계산
            lastAcceleration = Input.acceleration;

            if (deltaAcceleration.sqrMagnitude >= shakeThreshold * shakeThreshold)  // 변화량이 임계값 이상이면 흔들기로 간주
            {
                bool bResult = AddCount(1);

                if (bResult == false)
                {
                    Close();
                }

                Debug.Log("Shake detected!");
            }

            lastUpdateTime = Time.time;
        }

    }

    private bool AddCount(int nCount)
    {
        currentCount+=nCount;
        fillImage.fillAmount = (float)currentCount / targetCount;
        if (currentCount == 8)
        {
            fillImage.color = new Color(255 / 255f, 171 / 255f, 111 / 255f);
        }
        else if (currentCount == 13)
        {
            fillImage.color = new Color(255 / 255f, 111 / 255f, 111 / 255f);
        }
        else if (currentCount == targetCount)
        {
            return false;   //수치가 최대치인 경우 false
        }

        return true;
    }

    private void Close()
    {
        var hideUI = new HideUI { UINames = new List<string> { transform.name } };
        hideUI.ExecuteAsync().Forget();

        GameManager.Command.SwitchToNovelByShaker();
    }
}
