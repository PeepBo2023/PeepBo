using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel.UI;
using Naninovel;
using UnityEngine.EventSystems;
using PeepBo.Managers;

/* Naninovel에 있는 LabeledButton을 바로 상속받음
 * 버튼 클릭 효과만 추가
 * CustomUI가 아닌 기본 UI에 대응하기 위해 생성 <-> CustomPeepBoButton
 */
public class CustomLabeledButton : LabeledButton
{
	RectTransform rectTransform;
	Vector3 originScale;
	bool isClicked = false;

	protected override void Start()
	{
		base.Start();

		rectTransform = GetComponent<RectTransform>();
		originScale = rectTransform.localScale;
	}

    private void Update()
    {
        if (isClicked)
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originScale * 1.05f, 0.5f);
        else
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originScale, 0.5f);
    }

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		base.OnPointerDown(eventData);

		isClicked = true;
		GameManager.Audio.PlayClickSound();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		base.OnPointerUp(eventData);

		isClicked = false;
	}
}
