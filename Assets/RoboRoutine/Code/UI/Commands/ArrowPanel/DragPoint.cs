using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class DragPoint : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> BeginDragEvent;
        public event Action<PointerEventData> EndDragEvent;
        public event Action<PointerEventData> DragEvent;

        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _rectTransform;

        public Image Image => _image;
        public RectTransform RectTransform => _rectTransform;

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragEvent?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke(eventData);
        }
    }
}
