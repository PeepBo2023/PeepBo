using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PeepBo.Managers
{
    public struct HogamChoice
    {
        public string text;
        public string target;
        public int score;
        public HogamChoice(string _text, string _target, int _score)
        {
            text = _text;
            target = _target;
            score = _score;
        }

        public bool IsHogamChoice(string text) => this.text == text;
    }

    public class HogamManager
    {
        public async UniTask ShowHogamUI(HogamChoice hogamChoice, ChoiceHandlerButton buttonObject)
        {
            HogamComponent hogamComponent = buttonObject.GetComponentInChildren<HogamComponent>();
            hogamComponent.InitHogamComponent(hogamChoice);

            await hogamComponent.OnShow();

            await hogamComponent.Delay(0.4f);

            await hogamComponent.OnHide();
        }
    }
}
