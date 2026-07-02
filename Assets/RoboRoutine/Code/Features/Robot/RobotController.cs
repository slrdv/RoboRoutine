using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class RobotController : ITickListener, ISnapshotable, IDisposable
    {
        private readonly RobotView _robotView;
        private readonly ITickRegistry _tickRegistry;
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        private TickOperationBase _currentOperation;

        private readonly RobotMovementOperation _movementOperation;

        public SnapshotLayer SnapshotLayer => SnapshotLayer.Robot;

        public Vector2 GetPositionXZ()
        {
            return _robotView.GetPositionXZ();
        }

        public RobotController(RobotView robotView, ITickRegistry tickRegistry, ISnapshotableRegistry snapshotableRegistry)
        {
            _robotView = robotView;
            _tickRegistry = tickRegistry;
            _snapshotableRegistry = snapshotableRegistry;

            _tickRegistry.Add(this);
            _snapshotableRegistry.Register(this);

            _movementOperation = new RobotMovementOperation(_robotView);
        }

        public void Tick(float dt)
        {
            _currentOperation?.Tick(dt);
        }

        public UniTask<OperationResult> MoveXZAsync(Vector2 position)
        {
            _movementOperation.Setup(position);
            return RunOperation(_movementOperation);
        }

        public void StopCurrentOperation()
        {
            _currentOperation?.Stop();
        }

        public object CaptureState()
        {
            return new RobotState
            {
                Position = _robotView.GetPositionXZ(),
                Rotation = _robotView.GetRotation()
            };
        }

        public void RestoreState(object state)
        {
            if (state is not RobotState robotState) throw new ArgumentException($"Invalid state type: {state.GetType().Name}");

            _robotView.SetPositionXZ(robotState.Position);
            _robotView.SetRotation(robotState.Rotation);
        }

        public void Dispose()
        {
            _tickRegistry.Remove(this);
            _snapshotableRegistry.Remove(this);

            _currentOperation?.Stop();
        }

        private async UniTask<OperationResult> RunOperation(TickOperationBase operation)
        {
            if (_currentOperation != null) throw new InvalidOperationException($"Robot is busy");

            _currentOperation = operation;

            OperationResult result = await operation.Start();

            _currentOperation = null;

            return result;
        }
    }
}