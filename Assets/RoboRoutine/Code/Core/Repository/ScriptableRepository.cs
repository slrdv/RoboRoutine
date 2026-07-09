using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class ScriptableRepository<TKey, TItem> : IRepository<TKey, TItem> where TItem : ScriptableObject, IHasKey<TKey>
    {
        public int Count => _items.Count;

        private readonly string _configPath;

        private Dictionary<TKey, TItem> _items;
        private bool _loaded = false;

        public ScriptableRepository(string configPath)
        {
            _configPath = configPath;
        }

        public TItem Get(TKey key)
        {
            Load();
            return _items[key];
        }

        public bool TryGet(TKey key, out TItem item)
        {
            Load();
            return _items.TryGetValue(key, out item);
        }

        public IReadOnlyCollection<TItem> GetAll()
        {
            Load();
            return _items.Values;
        }

        public void Load()
        {
            if (_loaded) return;
            _loaded = true;

            TItem[] configs = Resources.LoadAll<TItem>(_configPath);
            _items = new Dictionary<TKey, TItem>(configs.Length);
            for (int i = 0; i < configs.Length; ++i)
            {
                _items[configs[i].Key] = configs[i];
            }
        }

        public bool Contains(TKey key)
        {
            Load();
            return _items.ContainsKey(key);
        }
    }
}
