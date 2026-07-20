using System;
using System.Collections.Generic;
using RoboRoutine.Core;
using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine.UI
{
    public sealed class ArrowPanelPresenter : IDisposable
    {
        private readonly ArrowPanelView _view;
        private readonly CommandListPresenter _commandList;
        private readonly ICanvasService _canvasService;
        private readonly Dictionary<Arrow, CommandItemModel> _arrows = new();

        public ArrowPanelPresenter(ArrowPanelView view, CommandListPresenter commandList, ICanvasService canvasService)
        {
            _view = view;
            _commandList = commandList;
            _canvasService = canvasService;
        }

        public void Initialize()
        {
            IReadOnlyList<CommandItemModel> items = _commandList.Model.Items;
            for (int i = 0; i < items.Count; i++)
            {
                if (HasTargetIndex(items[i]))
                {
                    AddItem(items[i]);
                }
            }

            UpdateArrows();

            _view.ArrowDropEvent += OnArrowDropped;

            _commandList.AfterItemCreatedEvent += OnItemAdded;
            _commandList.AfterItemRemovedEvent += OnItemRemoved;
            _commandList.AfterItemMovedEvent += OnItemMoved;
            _commandList.AfterItemsClearedEvent += OnItemsClear;
        }

        public void Dispose()
        {
            _view.ArrowDropEvent -= OnArrowDropped;

            _commandList.AfterItemCreatedEvent -= OnItemAdded;
            _commandList.AfterItemRemovedEvent -= OnItemRemoved;
            _commandList.AfterItemMovedEvent -= OnItemMoved;
            _commandList.AfterItemsClearedEvent -= OnItemsClear;

            Clear();
        }

        private void OnItemAdded(int index, CommandItemModel item)
        {
            if (HasTargetIndex(item))
            {
                AddItem(item);
            }

            UpdateArrows();
        }

        private void OnCommandDataChanged(CommandItemModel item)
        {
            if (TryGetTargetIndexData(item, out ITargetIndexCommandData data))
            {
                DrawArrow(GetArrow(item), item, data.TargetIndex);
            }
        }

        private void OnItemRemoved(int index, CommandItemModel removedItem)
        {
            if (HasTargetIndex(removedItem))
            {
                removedItem.DataChangedEvent -= OnCommandDataChanged;

                Arrow arrow = GetArrow(removedItem);
                _arrows.Remove(arrow);
                _view.RemoveArrow(arrow);
            }

            int allItemsCount = _commandList.Model.Count;

            foreach (var kv in _arrows)
            {
                Arrow arrow = kv.Key;
                CommandItemModel item = kv.Value;
                ITargetIndexCommandData indexCommandData = GetTargetIndexData(item);

                if (indexCommandData.TargetIndex >= allItemsCount)
                {
                    item.SetCommandData(indexCommandData.WithTargetIndex(-1));
                }
            }

            UpdateArrows();
        }

        private void OnItemMoved(int from, int to)
        {
            CommandItemModel item = _commandList.Model.Items[to];
            if (TryGetTargetIndexData(item, out ITargetIndexCommandData data))
            {
                DrawArrow(GetArrow(item), item, data.TargetIndex);
            }
        }

        private void OnItemsClear()
        {
            Clear();
        }

        private void OnArrowDropped(Arrow arrow, Vector2 position)
        {
            CommandItemModel item = _arrows[arrow];
            if (item.CommandData is not ITargetIndexCommandData data) return;

            if (_commandList.TryGetIndexAtPosition(position, out int targetIndex) && IsTargetIndexValid(item, targetIndex))
            {
                item.SetCommandData(data.WithTargetIndex(targetIndex));
            }
            else
            {
                _view.DrawDragPoint(arrow, GetItemPosition(item));
            }
        }

        private void AddItem(CommandItemModel item)
        {
            Arrow arrow = _view.CreateArrow();
            _arrows[arrow] = item;
            item.DataChangedEvent += OnCommandDataChanged;
        }

        private void Clear()
        {
            foreach (var kv in _arrows)
            {
                kv.Value.DataChangedEvent -= OnCommandDataChanged;
                _view.RemoveArrow(kv.Key);
            }
            _arrows.Clear();
        }

        private Vector2 GetItemPosition(CommandItemModel item, bool forceUpdate = false)
        {
            RectTransform rectTransform = _commandList.GetItemRect(item);
            return _canvasService.GetScreenPosition(rectTransform, forceUpdate);
        }

        private Arrow GetArrow(CommandItemModel item)
        {
            foreach (var kv in _arrows)
            {
                if (kv.Value == item)
                {
                    return kv.Key;
                }
            }

            return null;
        }

        private int GetItemIndex(CommandItemModel item)
        {
            return _commandList.GetItemIndex(item);
        }

        private bool TryGetTargetIndexData(CommandItemModel item, out ITargetIndexCommandData data)
        {
            if (item.CommandData is ITargetIndexCommandData targetIndexData)
            {
                data = targetIndexData;
                return true;
            }

            data = null;
            return false;
        }

        private int GetTargetIndex(CommandItemModel item)
        {
            return GetTargetIndexData(item).TargetIndex;
        }

        private bool HasTargetIndex(CommandItemModel item)
        {
            return item.CommandData is ITargetIndexCommandData;
        }

        private ITargetIndexCommandData GetTargetIndexData(CommandItemModel item)
        {
            return (ITargetIndexCommandData)item.CommandData;
        }

        private void DrawArrow(Arrow arrow, CommandItemModel item, int targetIndex)
        {
            Vector2 itemPosition = GetItemPosition(item);

            if (IsTargetIndexValid(item, targetIndex) && _commandList.TryGetPositionAtIndex(targetIndex, out Vector2 targetPosition))
            {
                _view.DrawArrow(arrow, itemPosition, targetPosition);
                return;
            }

            _view.DrawDragPoint(arrow, itemPosition);
        }

        private bool IsTargetIndexValid(CommandItemModel item, int targetIndex)
        {
            return targetIndex >= 0 && targetIndex < _commandList.Model.Count && GetItemIndex(item) != targetIndex;
        }

        private void UpdateArrows()
        {
            foreach (var kv in _arrows)
            {
                Arrow arrow = kv.Key;
                CommandItemModel item = kv.Value;
                DrawArrow(arrow, item, GetTargetIndex(item));
            }
        }
    }
}
