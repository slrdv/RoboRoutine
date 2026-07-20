using System;
using RoboRoutine.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoboRoutine.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class CommandItemView : PooledObjectBase<CommandItemView>, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<CommandItemView> BeginDragEvent;
        public event Action<CommandItemView, PointerEventData> DragEvent;
        public event Action<CommandItemView> EndDragEvent;
        public event Action ClickEvent;

        [SerializeField] private TMP_Text _index;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        public RectTransform RectTransform => _rectTransform;
        public CanvasGroup CanvasGroup => _canvasGroup;

        private bool _isDragging;
        private bool _isClickEnable = true;

        public void SetLabel(string label)
        {
            _label.text = label;
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetIconRotation(float rotation)
        {
            _icon.rectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotation));
        }

        public void SetIndex(int index)
        {
            _index.text = index.ToString();
        }

        public void SetIndexVisible(bool visible)
        {
            _index.gameObject.SetActive(visible);
        }

        public void CopyTo(CommandItemView other)
        {
            other._index.text = _index.text;
            other._index.gameObject.SetActive(_index.IsActive());
            other._label.text = _label.text;
            other._icon.sprite = _icon.sprite;
            other._icon.rectTransform.rotation = _icon.rectTransform.rotation;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            BeginDragEvent?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragEvent?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            EndDragEvent?.Invoke(this);
        }

        public void SetClickEnable(bool enabled)
        {
            _isClickEnable = enabled;
        }

        protected override void OnTaken()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected override void OnReleased()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (!_isClickEnable || _isDragging) return;

            ClickEvent?.Invoke();
        }
    }
}
