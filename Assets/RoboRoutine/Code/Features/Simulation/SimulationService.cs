using System;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class SimulationService : ISimulationService, ISimulationState, IDisposable
    {
        public event Action ResetEvent;
        public event Action StartEvent;
        public event Action StopEvent;
        public event Action FailedEvent;

        private readonly ICommandSequence _sequence;
        private readonly ICommandSequenceState _sequenceState;
        private readonly IHistoryService _history;

        private CommandResult _lastCommandResult = CommandResult.None;
        private bool _isRunning;
        private bool _runAll;

        public bool IsRunning => _isRunning;
        public int CommandIndex => _sequenceState.CommandIndex;
        public int CommandsCount => _sequenceState.Count;
        public bool IsInitialState => _history.SnapshotCount == 0;

        public SimulationService(ICommandSequence sequence, ICommandSequenceState sequenceState, IHistoryService history)
        {
            _sequence = sequence;
            _sequenceState = sequenceState;
            _history = history;

            _sequenceState.CommandCompleteEvent += OnCommandComplete;
            _sequenceState.SequenceUpdatedEvent += OnSequenceUpdated;
        }

        public void RunAll()
        {
            if (!CanRunNext()) return;

            _isRunning = true;
            _runAll = true;

            StartEvent?.Invoke();
            _sequence.ExecuteNext();
        }

        public void RunNext()
        {
            if (!CanRunNext()) return;

            _isRunning = true;
            _runAll = false;

            StartEvent?.Invoke();
            _sequence.ExecuteNext();
        }

        public void Undo()
        {
            if (!CanUndo()) return;

            _history.UndoLast();

            if (!IsInitialState)
            {
                StopEvent?.Invoke();
            }
            else
            {
                ResetEvent?.Invoke();
            }
        }

        public void Reset()
        {
            if (!CanReset()) return;

            if (_isRunning)
            {
                _sequence.Cancel();
                return;
            }

            _history.RollbackToInitial();
            _lastCommandResult = CommandResult.None;

            ResetEvent?.Invoke();
        }

        public bool CanRunNext()
        {
            return !IsRunning && _sequenceState.CommandIndex < _sequenceState.Count && (_lastCommandResult == CommandResult.None || _lastCommandResult == CommandResult.Success);
        }

        public bool CanUndo()
        {
            return !_isRunning && !IsInitialState;
        }

        public bool CanReset()
        {
            return !IsInitialState;
        }

        public void Dispose()
        {
            _sequenceState.CommandCompleteEvent -= OnCommandComplete;
            _sequenceState.SequenceUpdatedEvent -= OnSequenceUpdated;
        }

        private void OnCommandComplete(CommandResult result, int index)
        {
            _lastCommandResult = result;

            if (result == CommandResult.Canceled)
            {
                _isRunning = false;

                _history.RollbackToInitial();
                _lastCommandResult = CommandResult.None;
                ResetEvent?.Invoke();
            }
            else if (result == CommandResult.Success)
            {
                if (_runAll && _sequenceState.CommandIndex < _sequenceState.Count)
                {
                    StartEvent?.Invoke();
                    _sequence.ExecuteNext();
                }
                else
                {
                    _isRunning = false;
                    StopEvent?.Invoke();
                }
            }
            else if (result == CommandResult.Failed)
            {
                _isRunning = false;
                FailedEvent?.Invoke();
            }

            Debug.Log($"Command completed with result {result}");
        }

        private void OnSequenceUpdated()
        {
            _isRunning = false;
            _runAll = false;
            _lastCommandResult = CommandResult.None;

            _history.RollbackToInitial();

            ResetEvent?.Invoke();
        }
    }
}