using System;

namespace RoboRoutine
{
    public sealed class SimulationService : IDisposable
    {
        public event Action SimulationRunEvent;
        public event Action SimulationStopEvent;

        private readonly CommandSequence _sequence;
        private readonly HistoryService _history;

        private CommandResult _lastCommandResult = CommandResult.None;
        private bool _isRunning;
        private bool _runAll;

        public bool IsRunning => _isRunning;
        public int CommandIndex => _sequence.CommandIndex;
        public int CommandsCount => _sequence.Count;
        public bool IsInitialState => _history.SnapshotCount == 0;

        public SimulationService(CommandSequence sequence, HistoryService history)
        {
            _sequence = sequence;
            _history = history;

            _sequence.CommandCompleteEvent += OnCommandComplete;
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
            return !IsRunning && _sequence.CommandIndex < _sequence.Count && (_lastCommandResult == CommandResult.None || _lastCommandResult == CommandResult.Success);
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
            _sequence.CommandCompleteEvent -= OnCommandComplete;
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
                if (_runAll && _sequence.CommandIndex < _sequence.Count)
                {
                    _sequence.ExecuteNext();
                    SimulationRunEvent?.Invoke();
                }
                else
                {
                    _isRunning = false;
                }
            }

            SimulationStopEvent?.Invoke();
        }
    }
}