using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class RobotController : ITickListener, IDisposable
    {
        private readonly RobotView _robotView;
        private readonly ITickRegistry _tickRegistry;

        private TickOperationBase _currentOperation;

        private readonly RobotMovementOperation _movementOperation;


        public Vector2 GetPositionXZ()
        {
            return _robotView.GetPositionXZ();
        }

        public RobotController(RobotView robotView, ITickRegistry tickRegistry)
        {
            _robotView = robotView;
            _tickRegistry = tickRegistry;

            _tickRegistry.Add(this);

            _movementOperation = new RobotMovementOperation(_robotView);
        }

        public void Tick(float dt)
        {
            _currentOperation?.Tick(dt);
        }

        public UniTask MoveXZAsync(Vector2 position, CancellationToken ct)
        {
            _movementOperation.Setup(position);
            return RunOperation(_movementOperation, ct);
        }

        public void Dispose()
        {
            _tickRegistry.Remove(this);
            _currentOperation?.Cancel();
        }

        private async UniTask RunOperation(TickOperationBase operation, CancellationToken ct)
        {
            if (_currentOperation != null) throw new InvalidOperationException($"Robot is busy");

            _currentOperation = operation;

            await operation.Start(ct);

            _currentOperation = null;
        }
    }
}