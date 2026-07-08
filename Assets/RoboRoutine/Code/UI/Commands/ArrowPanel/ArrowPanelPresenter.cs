using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
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
            if (item.CommandData is ITargetIndexCommandData data)
            {
                Arrow arrow = _view.CreateArrow();
                _arrows[arrow] = item;
                item.DataChangedEvent += OnCommandDataChanged;

                DrawArrow(arrow, item, data.TargetIndex);
            }

            foreach (var kv in _arrows)
            {
                Arrow affectedArrow = kv.Key;
                CommandItemModel affectedItem = kv.Value;
                ITargetIndexCommandData indexCommandData = (ITargetIndexCommandData)affectedItem.CommandData;

                if (GetItemIndex(affectedItem) > index)
                {
                    DrawArrow(affectedArrow, affectedItem, indexCommandData.TargetIndex);
                }
            }

        }

        private void OnCommandDataChanged(CommandItemModel item)
        {
            if (item.CommandData is not ITargetIndexCommandData data) return;

            DrawArrow(GetArrow(item), item, data.TargetIndex);
        }

        private void OnItemRemoved(int index, CommandItemModel removedItem)
        {
            if (removedItem.CommandData is ITargetIndexCommandData data)
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
                ITargetIndexCommandData indexCommandData = (ITargetIndexCommandData)item.CommandData;

                if (indexCommandData.TargetIndex >= allItemsCount)
                {
                    item.SetCommandData(indexCommandData.WithTargetIndex(-1));
                }
                else if (GetItemIndex(item) >= index)
                {
                    DrawArrow(arrow, item, indexCommandData.TargetIndex);
                }
            }
        }

        private void OnItemMoved(int from, int to)
        {
            CommandItemModel item = _commandList.Model.Items[to];
            if (item.CommandData is not ITargetIndexCommandData data) return;
            DrawArrow(GetArrow(item), item, data.TargetIndex);
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
    }
}
