using System;
using Cysharp.Threading.Tasks;
using RoboRoutine.Core;
using UnityEngine;


namespace RoboRoutine.Features
{
    public sealed class RobotController : ITickListener, ISnapshotable, IDisposable
    {
        private readonly IRobotView _robotView;
        private readonly ITickRegistry _tickRegistry;
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        private readonly RobotMovementOperation _movementOperation;

        private TickOperationBase _currentOperation;
        private OperandEntityController _operand;

        public SnapshotLayer SnapshotLayer => SnapshotLayer.Robot;
        public OperandEntityController Operand => _operand;
        public IRobotView View => _robotView;

        public Vector2 GetPositionXZ()
        {
            return _robotView.GetPositionXZ();
        }

        public RobotController(IRobotView robotView, ITickRegistry tickRegistry, ISnapshotableRegistry snapshotableRegistry)
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
            _operand?.UpdateRotation();
        }

        public UniTask<OperationResult> MoveXZAsync(Vector2 position)
        {
            _movementOperation.Setup(position);
            return RunOperation(_movementOperation);
        }

        public void PickOperand(OperandEntityController operand)
        {
            _operand = operand;
            operand.AttachToParent(_robotView.ItemRoot, _robotView.ItemRoot.position, _robotView.ItemScale);
        }

        public OperandEntityController RemoveOperand()
        {
            OperandEntityController operand = _operand;
            _operand = null;
            return operand;
        }


        public void StopCurrentOperation()
        {
            _currentOperation?.Stop();
        }

        public object CaptureState()
        {
            return new RobotHistoryState
            {
                Position = _robotView.GetPositionXZ(),
                Rotation = _robotView.GetRotation(),
                Operand = _operand
            };
        }

        public void RestoreState(object state)
        {
            if (state is not RobotHistoryState robotState) throw new ArgumentException($"Invalid history state type: {state.GetType().Name}");

            _robotView.SetPositionXZ(robotState.Position);
            _robotView.SetRotation(robotState.Rotation);

            if (robotState.Operand != null && _operand == null)
            {
                PickOperand(robotState.Operand);
            }
            else if (robotState.Operand == null && _operand != null)
            {
                RemoveOperand();
            }
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
