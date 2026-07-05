using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class CommandSequence : ICommandSequence, ICommandSequenceState, IDisposable
    {
        public event Action<ICommand, int> BeforeCommandExecuteEvent;
        public event Action<CommandResult, int> CommandCompleteEvent;
        public event Action SequenceUpdatedEvent;

        private readonly List<ICommand> _commands = new();

        private int _commandIndex = 0;
        private bool _isExecuting;
        private bool _disposed;

        public IReadOnlyList<ICommand> Commands => _commands;
        public int CommandIndex => _commandIndex;
        public int Count => _commands.Count;
        public bool IsExecuting => _isExecuting;

        public void Add(ICommand command)
        {
            CheckIsBusy();
            _commands.Add(command);

            SequenceUpdatedEvent?.Invoke();
        }

        public void Insert(int index, ICommand command)
        {
            CheckIsBusy();
            _commands.Insert(index, command);

            SequenceUpdatedEvent?.Invoke();
        }

        public void RemoveAt(int index)
        {
            CheckIsBusy();
            _commands.RemoveAt(index);

            SequenceUpdatedEvent?.Invoke();
        }

        public void Move(int fromIndex, int toIndex)
        {
            CheckIsBusy();
            ICommand command = _commands[fromIndex];
            _commands.RemoveAt(fromIndex);
            _commands.Insert(toIndex, command);

            SequenceUpdatedEvent?.Invoke();
        }

        public void SetIndex(int index)
        {
            CheckIsBusy();
            _commandIndex = index;
        }

        public void Clear()
        {
            CheckIsBusy();
            
            _commands.Clear();
            _commandIndex = 0;

            SequenceUpdatedEvent?.Invoke();
        }

        public void ExecuteNext()
        {
            CheckIsBusy();
            ExecuteNextAsync().Forget();
        }

        public void Cancel()
        {
            if (_isExecuting)
            {
                _commands[_commandIndex].Cancel();
            }
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
            await UniTask.Yield();

            _isExecuting = false;
            
            if (_disposed) return;

            ++_commandIndex;

            CommandCompleteEvent?.Invoke(result, _commandIndex - 1);
        }

        private void CheckIsBusy()
        {
            if (_isExecuting) throw new InvalidOperationException("Sequence is busy");
        }
    }
}