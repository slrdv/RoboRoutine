using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class CommandSequence : IDisposable
    {
        public event Action<ICommand, int> BeforeCommandExecuteEvent;
        public event Action<CommandResult, int, int> CommandCompleteEvent;

        private readonly List<ICommand> _commands = new();

        private int _commandIndex = 0;
        private bool _isExecuting;
        private bool _disposed;

        public void Add(ICommand command)
        {
            CheckIsBusy();
            _commands.Add(command);
        }

        public void Insert(int index, ICommand command)
        {
            CheckIsBusy();
            _commands.Insert(index, command);
        }

        public void RemoveAt(int index)
        {
            CheckIsBusy();
            _commands.RemoveAt(index);
        }

        public void ExecuteNext()
        {
            CheckIsBusy();
            ExecuteNextAsync().Forget();
        }

        public void SetIndex(int index)
        {
            CheckIsBusy();
            _commandIndex = index;
        }

        public void Dispose()
        {
            _disposed = true;

            if (_isExecuting)
            {
                _commands[_commandIndex].Cancel();
            }
            _commands.Clear();
        }

        private async UniTaskVoid ExecuteNextAsync()
        {
            _isExecuting = true;

            ICommand command = _commands[_commandIndex];
            BeforeCommandExecuteEvent?.Invoke(command, _commandIndex);
            
            CommandResult result = await command.ExecuteAsync();

            _isExecuting = false;
            
            if (_disposed) return;

            ++_commandIndex;

            CommandCompleteEvent?.Invoke(result, _commandIndex - 1, _commands.Count);
        }

        private void CheckIsBusy()
        {
            if (_isExecuting) throw new InvalidOperationException("Sequence is busy");
        }
    }
}