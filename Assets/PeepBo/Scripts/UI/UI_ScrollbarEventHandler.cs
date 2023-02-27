using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PeepBo.UI
{
    public class UI_ScrollbarEventHandler : UI_EventHandler
    {
        ScrollRect scrollRect = null;
        bool isDrag = false;

        public void SetScrollRect(ScrollRect scrollRect) => this.scrollRect = scrollRect;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
            base.OnBeginDrag(eventData);
            scrollRect.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            scrollRect.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            isDrag = false;
            scrollRect.OnEndDrag(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (isDrag) return;
            base.OnPointerClick(eventData);
        }
    }
}