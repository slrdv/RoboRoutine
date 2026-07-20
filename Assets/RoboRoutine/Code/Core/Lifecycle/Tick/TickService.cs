using System.Collections.Generic;
using VContainer.Unity;

namespace RoboRoutine.Core
{
    public sealed class TickService : ITickService, ITickRegistry, ITickable
    {
        private readonly List<ITickListener> _tickListeners = new();
        private float _timeScale = 1f;
        private bool _paused;

        public void SetTimeScale(float timeScale)
        {
            _timeScale = timeScale;
        }

        public void Start()
        {
            _paused = false;
        }

        public void Stop()
        {
            _paused = true;
        }

        public void Add(ITickListener tickListener)
        {
            if (!_tickListeners.Contains(tickListener))
            {
                _tickListeners.Add(tickListener);
            }
        }
        public void Remove(ITickListener tickListener)
        {
            _tickListeners.Remove(tickListener);
        }

        public void Tick()
        {
            if (_paused) return;

            float dt = UnityEngine.Time.deltaTime * _timeScale;

            for (int i = 0; i < _tickListeners.Count; i++)
            {
                _tickListeners[i].Tick(dt);
            }
        }
    }
}
