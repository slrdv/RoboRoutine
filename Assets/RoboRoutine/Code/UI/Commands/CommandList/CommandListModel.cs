using System;
using System.Collections.Generic;
using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class CommandListModel : IDisposable
    {
        public event Action<int, CommandItemModel> ItemAddedEvent;
        public event Action<int, CommandItemModel> ItemRemovedEvent;
        public event Action<int, int> ItemMovedEvent;
        public event Action<int, CommandData> ItemDataChangedEvent;
        public event Action ClearedEvent;
        public event Action ChangedEvent;

        private readonly List<CommandItemModel> _items = new();

        public IReadOnlyList<CommandItemModel> Items => _items;
        public int Count => _items.Count;

        public void Add(CommandItemModel item)
        {
            Insert(item, _items.Count);
        }

        public int GetIndex(CommandItemModel item)
        {
            return _items.IndexOf(item);
        }

        public void Remove(CommandItemModel item)
        {
            Remove(_items.IndexOf(item));
        }

        public void Remove(int index)
        {
            CommandItemModel item = _items[index];
            item.DataChangedEvent -= OnItemDataChanged;
            _items.RemoveAt(index);
            UpdateIndexes();

            ItemRemovedEvent?.Invoke(index, item);
            ChangedEvent?.Invoke();
        }

        public void Move(int from, int to)
        {
            if (from == to) return;

            CommandItemModel item = _items[from];
            _items.RemoveAt(from);
            _items.Insert(to, item);
            UpdateIndexes();

            ItemMovedEvent?.Invoke(from, to);
            ChangedEvent?.Invoke();
        }

        public void Insert(CommandItemModel item, int index)
        {
            _items.Insert(index, item);
            item.DataChangedEvent += OnItemDataChanged;
            UpdateIndexes();

            ItemAddedEvent?.Invoke(index, item);
            ChangedEvent?.Invoke();
        }

        public void RemoveAll()
        {
            Clear();

            ClearedEvent?.Invoke();
            ChangedEvent?.Invoke();
        }

        public void Dispose()
        {
            Clear();
        }

        private void Clear()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].DataChangedEvent -= OnItemDataChanged;
            }
            _items.Clear();
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SetIndex(i);
            }
        }

        private void OnItemDataChanged(CommandItemModel model)
        {
            ItemDataChangedEvent?.Invoke(_items.IndexOf(model), model.CommandData);
            ChangedEvent?.Invoke();
        }
    }
}
