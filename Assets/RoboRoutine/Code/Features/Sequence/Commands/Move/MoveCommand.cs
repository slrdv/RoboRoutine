using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class MoveCommand : ICommand, IDisposable
    {
        public CommandType CommandType => CommandType.Move;

        private readonly MovementSystem _movementSystem;
        private readonly MoveDirection _direction;
        private readonly int _distance;

        private CancellationTokenSource _cts;
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

            _cts = new CancellationTokenSource();

            CommandResult result = await _movementSystem.MoveAsync(_direction, _distance, _cts.Token);

            _cts?.Dispose();
            _cts = null;
            _isExecuting = false;

            return result;
        }

        public void Cancel()
        {
            _cts?.Cancel();
        }

        public void Dispose()
        {
            Cancel();
        }
    }
}