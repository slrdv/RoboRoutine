using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public abstract class TickOperationBase
    {
        private UniTaskCompletionSource _tcs;
        private CancellationTokenRegistration _ctr;

        private bool _isRunning = false;

        public UniTask Start(CancellationToken ct = default)
        {
            if (_isRunning) throw new InvalidOperationException($"[{GetType().Name}] Already running");

            _tcs = new UniTaskCompletionSource();
            _ctr = ct.RegisterWithoutCaptureExecutionContext(Cancel);

            _isRunning = true;

            OnStart();

            return _tcs.Task;
        }

        public void Tick(float dt)
        {
            if (!_isRunning) return;

            OnTick(dt);
        }

        public void Cancel()
        {
            Detach()?.TrySetCanceled();
        }

        protected abstract void OnStart();
        protected abstract void OnTick(float dt);

        protected void Complete()
        {
            Detach()?.TrySetResult();
        }

        protected void Fail(Exception ex)
        {
            Detach()?.TrySetException(ex);
        }

        private UniTaskCompletionSource Detach()
        {
            if (!_isRunning) return null;

            UniTaskCompletionSource tcs = _tcs;
            _tcs = null;
            _ctr.Dispose();

            _isRunning = false;

            return tcs;
        }
    }
}