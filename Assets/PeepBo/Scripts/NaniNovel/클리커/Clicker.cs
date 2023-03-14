using DG.Tweening;
using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    [SerializeField] private int targetCount;
    [SerializeField] private Image fillImage;

    public GameObject pivot;
    public Image back;

    int currentCount = 0;
    bool isShow = false;

    Tweener tweenerPivot;
    Tweener tweenerBack;

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

        tweenerPivot = pivot.transform.DOShakePosition(1000.0f, 3.5f, 10, 180, false, false);
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

            if (tweenerBack != null)
            {
                if (tweenerBack.IsPlaying())
                {
                    tweenerBack.Kill();
                    back.transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
                }

            }
            tweenerBack = back.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f) * -1, 0.5f);

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
                var hideUI = new HideUI { UINames = new List<string> { transform.name } };
                hideUI.ExecuteAsync().Forget();

                tweenerBack.Kill();
                tweenerPivot.Kill();

                GameManager.Command.SwitchToNovelByClicker();
            }
        };
    }
}
