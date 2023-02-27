using Naninovel;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PeepBo.Nani
{
    public class PeepboChoiceHandlerButton : ChoiceHandlerButton
    {
        public override void Initialize(ChoiceState choiceState)
        {
            bool isInteractable = GameManager.Choice.IsInteractable(choiceState.Summary);
            if (!isInteractable)
            {
                Interactable = false;
                var image = gameObject.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 125f/255f);
            }
            base.Initialize(choiceState);
        }

        public void OnPress()
        {
            GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        public void DePress()
        {
            GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
}