using UnityEngine;

namespace RoboRoutine
{
    public abstract class PooledObjectBase<TSelf> : MonoBehaviour where TSelf : PooledObjectBase<TSelf>
    {
        private GameObjectPool<TSelf> _pool = null;

        private bool _isAcquired = false;

        public void SetPool(GameObjectPool<TSelf> pool)
        {
            if (_pool != null)
            {
                Debug.LogError($"Pool is already set for {typeof(TSelf).Name}");
                return;
            }
            _pool = pool;
        }

        public void OnTakenFromPool()
        {
            if (_isAcquired)
            {
                Debug.LogError($"[{name}] ({GetType().Name}) is already acquired", this);
                return;
            }

            OnTaken();

            _isAcquired = true;
            SetActive(true);
        }

        public void OnReleasedToPool()
        {
            if (!_isAcquired)
            {
                Debug.LogError($"[{name}] ({GetType().Name}) is not acquired", this);
                return;
            }

            OnReleased();

            _isAcquired = false;
            SetActive(false);
        }

        public void Release()
        {
            if (_pool == null)
            {
                Debug.LogError($"[{name}] ({GetType().Name}) pool reference is null", this);
                return;
            }
            _pool.Release((TSelf)this);
        }

        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        protected virtual void OnTaken() { }
        protected virtual void OnReleased() { }
    }
}
