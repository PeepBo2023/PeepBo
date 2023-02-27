using BackEnd;
using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PeepBo.Nani
{
    [ActorResources(typeof(ChoiceHandlerPanel), false)]
    public class PeepboUIChoiceHandler : UIChoiceHandler
    {
        public PeepboUIChoiceHandler(string id, ChoiceHandlerMetadata metadata) : base(id, metadata)
        {
            
        }

        protected override async void HandleChoice(ChoiceState state, ChoiceHandlerButton buttonObject)
        {
            await GameManager.Choice.SelectChoice(state, buttonObject);
            base.HandleChoice(state, buttonObject);
        }
    }
}