using System;
using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public abstract class TickOperationBase
    {
        private UniTaskCompletionSource<OperationResult> _tcs;

        private bool _isRunning = false;

        public UniTask<OperationResult> Start()
        {
            if (_isRunning) throw new InvalidOperationException($"[{GetType().Name}] Already running");

            _tcs = new UniTaskCompletionSource<OperationResult>();
            _isRunning = true;

            OnStart();

            return _tcs.Task;
        }

        public void Tick(float dt)
        {
            if (!_isRunning) return;

            OnTick(dt);
        }

        public void Stop()
        {
            Detach()?.TrySetResult(OperationResult.Cancelled);
        }

        protected abstract void OnStart();
        protected abstract void OnTick(float dt);

        protected void Complete()
        {
            Detach()?.TrySetResult(OperationResult.Complete);
        }

        protected void Fail(Exception ex)
        {
            Detach()?.TrySetException(ex);
        }

        private UniTaskCompletionSource<OperationResult> Detach()
        {
            if (!_isRunning) return null;

            UniTaskCompletionSource<OperationResult> tcs = _tcs;
            _tcs = null;

            _isRunning = false;

            return tcs;
        }
    }
}
