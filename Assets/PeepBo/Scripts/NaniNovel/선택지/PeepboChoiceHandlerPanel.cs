using Naninovel;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PeepBo.Nani
{
    public class PeepboChoiceHandlerPanel : ChoiceHandlerPanel
    {
        protected override void Start()
        {
            GameManager.Choice.HandlerPanel = this;
        }

        public void OnClickChoice() // 선택지들 다 비활성화
        {
            var contentPanel = transform.GetChild(0);

            for (int i = 0; i < contentPanel.childCount; i++)
            {
                var peepboChoiceHandlerButton = contentPanel.GetChild(i).GetComponent<Button>(); // 버튼
                var eventTrigger = contentPanel.GetChild(i).GetComponent<EventTrigger>(); // 텍스트 효과

                peepboChoiceHandlerButton.interactable = false;
                eventTrigger.enabled = false;
            }
        }
    }
}