using Naninovel;
using PeepBo.Managers;
using PeepBo.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        var inputManager = Engine.GetService<IInputManager>();

        if (scriptPlayer.Playing || inputManager.ProcessInput) return;

        var tutorial = GameManager.UI.ShowPopupUI<UI_RoomTutorialPopup>();
    }
}
