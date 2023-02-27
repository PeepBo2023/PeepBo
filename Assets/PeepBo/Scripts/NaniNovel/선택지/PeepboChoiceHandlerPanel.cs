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

        public void OnClickChoice() // �������� �� ��Ȱ��ȭ
        {
            var contentPanel = transform.GetChild(0);

            for (int i = 0; i < contentPanel.childCount; i++)
            {
                var peepboChoiceHandlerButton = contentPanel.GetChild(i).GetComponent<Button>(); // ��ư
                var eventTrigger = contentPanel.GetChild(i).GetComponent<EventTrigger>(); // �ؽ�Ʈ ȿ��

                peepboChoiceHandlerButton.interactable = false;
                eventTrigger.enabled = false;
            }
        }
    }
}