using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoboRoutine
{
    public sealed class CommandPaletteView : MonoBehaviour
    {
        public event Action BeginDragEvent;
        public event Action<PointerEventData> DragEvent;
        public event Action<int> EndDragEvent;

        [SerializeField] private RectTransform _itemRoot;
        [SerializeField] private CommandItemView _ghostItem;

        private bool _isDragging;

        private readonly List<CommandItemView> _items = new();

        public void AddItem(CommandItemView item)
        {
            item.transform.SetParent(_itemRoot);
            _items.Add(item);

            item.BeginDragEvent += OnBeginDrag;
            item.DragEvent += OnDrag;
            item.EndDragEvent += OnEndDrag;
        }

        public void ToggleActive()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void OnBeginDrag(CommandItemView item)
        {
            if (_isDragging) return;
            _isDragging = true;

            _ghostItem.RectTransform.position = item.RectTransform.position;
            item.CopyTo(_ghostItem);
            _ghostItem.gameObject.SetActive(true);

            BeginDragEvent?.Invoke();
        }

        private void OnDrag(CommandItemView itemView, PointerEventData eventData)
        {
            if (!_isDragging) return;
            _ghostItem.RectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0f);

            DragEvent?.Invoke(eventData);
        }

        private void OnEndDrag(CommandItemView item)
        {
            _isDragging = false;
            _ghostItem.gameObject.SetActive(false);

            EndDragEvent?.Invoke(_items.IndexOf(item));
        }

        private void Awake()
        {
            _ghostItem.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                CommandItemView item = _items[i];

                item.BeginDragEvent -= OnBeginDrag;
                item.DragEvent -= OnDrag;
                item.EndDragEvent -= OnEndDrag;
            }

            _items.Clear();
        }
    }
}
