using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoboRoutine
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class CommandItemView : PooledObjectBase<CommandItemView>, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<CommandItemView> BeginDragEvent;
        public event Action<CommandItemView, PointerEventData> DragEvent;
        public event Action<CommandItemView> EndDragEvent;

        [SerializeField] private TMP_Text _index;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Image _icon;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        public RectTransform RectTransform => _rectTransform;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void SetLabel(string label)
        {
            _label.text = label;
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetIndex(int index)
        {
            _index.text = index.ToString();
        }

        public void CopyTo(CommandItemView other)
        {
            other._index.text = _index.text;
            other._label.text = _label.text;
            other._icon.sprite = _icon.sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragEvent?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke(this);
        }
    }
}