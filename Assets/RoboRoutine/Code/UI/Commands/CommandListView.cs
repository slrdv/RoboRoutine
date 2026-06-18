using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoboRoutine
{
    public sealed class CommandListView : MonoBehaviour
    {
        public event Action<int, int> ItemDraggedEvent;

        [SerializeField] private RectTransform _itemRoot;
        [SerializeField] private RectTransform _dragLayer;
        [SerializeField] private CommandItemView _ghostItem;
        [SerializeField] private RectTransform _dropLine;
        [SerializeField] private RectTransform _pointer;

        private readonly List<CommandItemView> _items = new();

        private bool _isDragging;
        private int _originIndex;
        private int _dropIndex;
        private bool _inputEnabled = true;

        public void AddItem(CommandItemView item)
        {
            item.BeginDragEvent += OnBeginDrag;
            item.DragEvent += OnDrag;
            item.EndDragEvent += OnEndDrag;

            item.transform.SetParent(_itemRoot, false);
            _items.Add(item);
        }

        public void RemoveItem(CommandItemView item)
        {
            _items.Remove(item);

            item.BeginDragEvent -= OnBeginDrag;
            item.DragEvent -= OnDrag;
            item.EndDragEvent -= OnEndDrag;
        }

        public void MoveItem(int fromIndex, int toIndex)
        {
            CommandItemView item = _items[fromIndex];
            _items.RemoveAt(fromIndex);
            _items.Insert(toIndex, item);
            item.RectTransform.SetSiblingIndex(toIndex);
        }

        public void ShowPointer(int itemIndex)
        {
            if (_items.Count == 0) return;
            
            Vector3 position = _pointer.position;
            position.y = _items[itemIndex].RectTransform.position.y;
            _pointer.position = position;
            _pointer.gameObject.SetActive(true);
        }

        public void HidePointer()
        {
            _pointer.gameObject.SetActive(false);
        }

        public void SetInputEnabled(bool enabled)
        {
            _inputEnabled = enabled;
        } 

        public void ClearItems()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                RemoveItem(_items[i]);
            }
        }

        private void Awake()
        {
            HideGhostItem();
            HideDropLine();
            HidePointer();

            foreach (Transform child in _itemRoot)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnBeginDrag(CommandItemView item)
        {
            if (_isDragging || !_inputEnabled) return;

            _originIndex = _items.IndexOf(item);
            _dropIndex = _originIndex;

            ShowGhostItem(item);
            ShowDropLine(_originIndex);

            item.CanvasGroup.alpha = 0.35f;
            item.CanvasGroup.blocksRaycasts = false;

            _isDragging = true;
        }

        private void OnDrag(CommandItemView item, PointerEventData eventData)
        {
            if (!_isDragging || !_inputEnabled) return;

            _ghostItem.RectTransform.position += new Vector3(0f, eventData.delta.y, 0f);

            _dropIndex = GetDropIndex();
            ShowDropLine(_dropIndex > _originIndex ? _dropIndex + 1 : _dropIndex);
        }

        private void OnEndDrag(CommandItemView item)
        {
            if (!_isDragging || !_inputEnabled) return;

            HideDropLine();
            HideGhostItem();

            item.CanvasGroup.alpha = 1f;
            item.CanvasGroup.blocksRaycasts = true;

            _isDragging = false;

            if (_originIndex != _dropIndex)
            {
                ItemDraggedEvent?.Invoke(_originIndex, _dropIndex);
            }
        }

        private int GetDropIndex()
        {
            float dragY = _ghostItem.RectTransform.position.y;

            for (int i = 0; i < _items.Count; i++)
            {
                if (dragY > _items[i].RectTransform.position.y)
                {
                    if (i == _originIndex || i == _originIndex + 1) return _originIndex;
                    return i > _originIndex ? i - 1 : i;
                }
            }

            return _items.Count == _originIndex + 1 ? _originIndex : _items.Count - 1;
        }

        private void ShowDropLine(int index)
        {
            _dropLine.SetParent(_itemRoot, false);
            _dropLine.gameObject.SetActive(true);
            _dropLine.SetSiblingIndex(index);
        }

        private void HideDropLine()
        {
            _dropLine.gameObject.SetActive(false);
            _dropLine.SetParent(_dragLayer, false);
        }

        private void ShowGhostItem(CommandItemView item)
        {
            _ghostItem.RectTransform.position = item.RectTransform.position;
            item.CopyTo(_ghostItem);
            _ghostItem.gameObject.SetActive(true);
        }

        private void HideGhostItem()
        {
            _ghostItem.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ClearItems();
        }
    }
}