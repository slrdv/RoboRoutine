using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine.Core
{
    public sealed class GameObjectPool<T> where T : PooledObjectBase<T>
    {
        private readonly T _prefab;
        private readonly Transform _container;
        private readonly IObjectResolver _objectResolver;
        private readonly int _maxPoolSize;

        private readonly Stack<T> _pool;

        public GameObjectPool(IObjectResolver objectResolver, T prefab, Transform container, int maxPoolSize = 100, int defaultCapacity = 10)
        {
            _objectResolver = objectResolver;
            _prefab = prefab;
            _container = container;
            _maxPoolSize = maxPoolSize;

            _pool = new Stack<T>(defaultCapacity);
        }

        public T Get(bool active = true)
        {
            T obj = _pool.Count > 0 ? _pool.Pop() : Create();
            obj.OnTakenFromPool(active);
            return obj;
        }

        public void Release(T obj)
        {
            obj.OnReleasedToPool();
            obj.transform.SetParent(_container);

            if (obj.transform is not RectTransform)
            {
                obj.transform.position = _container.position;
            }

            if (_pool.Count >= _maxPoolSize)
            {
                Object.Destroy(obj.gameObject);
                return;
            }

            _pool.Push(obj);
        }

        public void Prewarm(int amount)
        {
            amount = Mathf.Min(amount, _maxPoolSize - _pool.Count);
            for (int i = 0; i < amount; ++i)
            {
                T obj = Create();
                obj.OnTakenFromPool(false);
                Release(obj);
            }
        }

        private T Create()
        {
            T obj = _objectResolver.Instantiate(_prefab, _container);
            obj.SetPool(this);
            return obj;
        }
    }
}
