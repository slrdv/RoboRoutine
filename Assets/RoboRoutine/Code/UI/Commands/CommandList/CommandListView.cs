using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace RoboRoutine
{
    public sealed class CommandListView : MonoBehaviour
    {
        public event Action<int, int> ItemDroppedEvent;
        public event Action<int, CommandData> ExternalItemDroppedEvent;
        public event Action<int> ItemDroppedOutEvent;

        [SerializeField] private RectTransform _itemRoot;
        [SerializeField] private RectTransform _dragLayer;
        [SerializeField] private CommandItemView _ghostItem;
        [SerializeField] private RectTransform _dropLine;
        [SerializeField] private RectTransform _pointer;
        [SerializeField] private RectTransform _layoutRect;
        private ICanvasService _canvasService;

        private readonly List<CommandItemView> _items = new();

        private bool _isDragging;
        private bool _isExternalDragging;
        private int _originIndex;
        private int _dropIndex;
        private bool _inputEnabled = true;

        [Inject]
        public void Construct(ICanvasService canvasService)
        {
            _canvasService = canvasService;
        }

        public void AddItem(CommandItemView item, int index)
        {
            item.BeginDragEvent += OnBeginDrag;
            item.DragEvent += OnDrag;
            item.EndDragEvent += OnEndDrag;

            item.transform.SetParent(_itemRoot, false);
            item.RectTransform.SetSiblingIndex(index);
            _items.Insert(index, item);
        }

        public void RemoveItem(int index)
        {
            CommandItemView item = _items[index];
            _items.RemoveAt(index);

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
                RemoveItem(i);
            }
        }

        public bool TryGetIndexAtPosition(Vector2 screenPosition, out int result)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_canvasService.IsPointOverRect(_items[i].RectTransform, screenPosition))
                {
                    result = i;
                    return true;
                }
            }

            result = -1;
            return false;
        }

        public bool TryGetPositionAtIndex(int index, out Vector2 result)
        {
            if (index < 0 || index >= _items.Count)
            {
                result = default;
                return false;
            }

            result = _canvasService.GetScreenPosition(_items[index].RectTransform);
            return true;
        }

        public void OnExternalDrag(PointerEventData eventData)
        {
            if (!_inputEnabled) return;

            if (!_canvasService.IsPointOverRect(_itemRoot, eventData.position))
            {
                if (_isExternalDragging)
                {
                    _isExternalDragging = false;
                    HideDropLine();
                }
                return;
            }

            _isExternalDragging = true;

            _dropIndex = GetDropIndex(eventData.position.y);
            ShowDropLine(_dropIndex);
        }

        public void OnExternalEndDrag(CommandData commandData)
        {
            HideDropLine();

            if (!_isExternalDragging) return;
            _isExternalDragging = false;

            ExternalItemDroppedEvent?.Invoke(_dropIndex, commandData);
        }

        public void UpdateLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutRect);
            _canvasService.ForceUpdate();
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
            if (!_inputEnabled || _isDragging || _isExternalDragging) return;

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

            _ghostItem.RectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0f);
            Vector2 position = _ghostItem.RectTransform.position;

            if (!_canvasService.IsPointOverRect(_itemRoot, position))
            {
                HideDropLine();
                return;
            }

            _dropIndex = GetDropIndex(position.y);

            if (_dropIndex == _originIndex || _dropIndex == _originIndex + 1)
            {
                _dropIndex = _originIndex;
            }
            else if (_dropIndex > _originIndex)
            {
                _dropIndex -= 1;
            }

            ShowDropLine(_dropIndex > _originIndex ? _dropIndex + 1 : _dropIndex);
        }

        private void OnEndDrag(CommandItemView item)
        {
            if (!_isDragging || !_inputEnabled) return;

            item.CanvasGroup.alpha = 1f;
            item.CanvasGroup.blocksRaycasts = true;

            _isDragging = false;

            if (!_canvasService.IsPointOverRect(_itemRoot, _ghostItem.RectTransform.position))
            {
                ItemDroppedOutEvent?.Invoke(_originIndex);
            }
            else if (_originIndex != _dropIndex)
            {
                ItemDroppedEvent?.Invoke(_originIndex, _dropIndex);
            }

            HideDropLine();
            HideGhostItem();
        }

        private int GetDropIndex(float dragY)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (dragY > _items[i].RectTransform.position.y)
                {
                    return i;
                }
            }

            return _items.Count;
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