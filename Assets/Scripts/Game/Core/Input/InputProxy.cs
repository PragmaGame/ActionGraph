using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.Input
{
    public class InputProxy : Graphic, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public event Action PointerDownEvent;
        public event Action PointerUpEvent;
        public event Action PointerDragEvent;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDownEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUpEvent?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            PointerDragEvent?.Invoke();
        }
    }
}