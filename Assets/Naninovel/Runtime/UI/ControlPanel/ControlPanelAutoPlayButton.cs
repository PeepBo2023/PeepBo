// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.UI
{
    public class ControlPanelAutoPlayButton : ScriptableLabeledButton
    {
        //[SerializeField] private Color activeColorMultiplier = Color.red;
        [SerializeField] private Sprite autoPlayOff;
        [SerializeField] private Sprite autoPlayOn;

        private IScriptPlayer player;
        private Image buttonImage;

        protected override void Awake ()
        {
            base.Awake();

            player = Engine.GetService<IScriptPlayer>();
            buttonImage = GetComponent<Image>();
        }

        protected override void OnEnable ()
        {
            base.OnEnable();
            player.OnAutoPlay += HandleAutoModeChange;
        }

        protected override void OnDisable ()
        {
            base.OnDisable();
            player.OnAutoPlay -= HandleAutoModeChange;
        }

        protected override void OnButtonClick ()
        {
            player.SetAutoPlayEnabled(!player.AutoPlayActive);
        }

        private void HandleAutoModeChange (bool enabled)
        {
            //UIComponent.LabelColorMultiplier = enabled ? activeColorMultiplier : Color.white;
            buttonImage.sprite = enabled ? autoPlayOn : autoPlayOff;
        }
    } 
}
