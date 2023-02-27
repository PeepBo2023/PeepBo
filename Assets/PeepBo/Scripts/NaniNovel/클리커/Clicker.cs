using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    [SerializeField] private int targetCount;
    [SerializeField] private Image fillImage;

    int currentCount = 0;
    bool isShow = false;

    private void Start()
    {
        fillImage.fillAmount = (float)currentCount / targetCount;
        var customUI = GetComponent<CustomUI>();
        customUI.OnVisibilityChanged += CustomUI_OnVisibilityChanged;
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
        if (Input.GetMouseButtonDown(0) && isShow)
        {
            currentCount++;
            fillImage.fillAmount = (float)currentCount / targetCount;
            if (currentCount == 8)
            {
                fillImage.color=new Color(255/255f, 171/255f, 111/255f);
            }
            else if (currentCount == 13)
            {
                fillImage.color = new Color(255/255f, 111/255f, 111/255f);
            }
            else if (currentCount == targetCount)
            {
                var hideUI = new HideUI { UINames = new List<string> { transform.name } };
                hideUI.ExecuteAsync().Forget();

                GameManager.Command.SwitchToNovelByClicker();
            }
        };
    }
}
