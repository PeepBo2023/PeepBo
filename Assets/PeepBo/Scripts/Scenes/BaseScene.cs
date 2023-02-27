using UnityEngine;
using UnityEngine.EventSystems;
using PeepBo.Managers;
using System;

namespace PeepBo.Scene
{
    public class BaseScene : MonoBehaviour
    {
        protected Action OnDestroyAction = null;

        protected virtual void Init()
        {
            UnityEngine.Object eventSystem = GameObject.FindObjectOfType(typeof(EventSystem));
            if (eventSystem == null)
            {
                eventSystem = GameManager.Resource.Instantiate("EventSystem");
                eventSystem.name = "@EventSystem";
                DontDestroyOnLoad(eventSystem);
            }
        }

        private void OnDestroy()
            => OnDestroyAction?.Invoke();

        public void AddOnDestroyAction(Action action)
            => OnDestroyAction += action;
        //public abstract void Clear();
    }
}
