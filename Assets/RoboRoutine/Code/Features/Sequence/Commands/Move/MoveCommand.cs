using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class MoveCommand : ICommand, IDisposable
    {
        public event Action<CommandResult> OnComplete;

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

        public void Execute()
        {
            if (_isExecuting)
            {
                Debug.LogWarning("MoveCommand is already executing");
                return;
            }
            
            _isExecuting = true;

            _cts = new CancellationTokenSource();
            ExecuteAsync(_cts.Token).Forget();
        }

        public void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void Dispose()
        {
            Cancel();
        }

        private async UniTaskVoid ExecuteAsync(CancellationToken ct)
        {
            CommandResult result = await _movementSystem.MoveAsync(_direction, _distance, ct);
            
            _cts?.Dispose();
            _cts = null;
            _isExecuting = false;

            OnComplete?.Invoke(result);
        }
    }
}