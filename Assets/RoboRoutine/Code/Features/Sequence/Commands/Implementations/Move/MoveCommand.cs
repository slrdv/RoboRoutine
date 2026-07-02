using System;
using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class MoveCommand : ICommand, IDisposable
    {
        public CommandType CommandType => CommandType.Move;
        public SnapshotLayer SnapshotLayer => SnapshotLayer.Robot;

        private readonly MovementSystem _movementSystem;
        private readonly MoveDirection _direction;
        private readonly int _distance;
        private bool _isExecuting;

        public MoveCommand(MovementSystem movementSystem, MoveDirection direction, int distance)
        {
            _movementSystem = movementSystem;
            _direction = direction;
            _distance = distance;
        }

        public async UniTask<CommandResult> ExecuteAsync()
        {
            if (_isExecuting) throw new InvalidOperationException("MoveCommand is already executing");
            _isExecuting = true;

            CommandResult result = await _movementSystem.MoveAsync(_direction, _distance);

            _isExecuting = false;

            return result;
        }

        public void Cancel()
        {
            _movementSystem.Stop();
        }

        public void Dispose()
        {
            Cancel();
        }
    }
}