using System;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class SimulationService : ISimulationService, ISimulationState, IDisposable
    {
        public event Action SimulationRunEvent;
        public event Action SimulationStopEvent;

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

            _sequence.ExecuteNext();
            SimulationRunEvent?.Invoke();
        }

        public void RunNext()
        {
            if (!CanRunNext()) return;

            _isRunning = true;
            _runAll = false;

            _sequence.ExecuteNext();
            SimulationRunEvent?.Invoke();
        }

        public void Back()
        {
            if (!CanBack()) return;

            _history.UndoLast();
            SimulationStopEvent?.Invoke();
        }

        public void Stop()
        {
            if (!CanStop()) return;

            if (_isRunning)
            {
                _sequence.Cancel();
                return;
            }

            _history.RollbackToInitial();
            SimulationStopEvent?.Invoke();
        }

        public bool CanRunNext()
        {
            return !IsRunning && _sequenceState.CommandIndex < _sequenceState.Count && (_lastCommandResult == CommandResult.None || _lastCommandResult == CommandResult.Success);
        }

        public bool CanBack()
        {
            return !_isRunning && !IsInitialState;
        }

        public bool CanStop()
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
            }
            else if (result == CommandResult.Success)
            {
                if (_runAll && _sequenceState.CommandIndex < _sequenceState.Count)
                {
                    _sequence.ExecuteNext();
                    SimulationRunEvent?.Invoke();
                }
                else
                {
                    _isRunning = false;
                }
            }
            
            Debug.Log($"Command completed with result {result}");
            SimulationStopEvent?.Invoke();
        }

        private void OnSequenceUpdated()
        {
            if (!IsInitialState)
            {
                _isRunning = false;
                _runAll = false;
                _lastCommandResult = CommandResult.None;

                _history.RollbackToInitial();
            }

            SimulationStopEvent?.Invoke();
        }
    }
}