using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class HistoryService : IDisposable
    {
        private readonly CommandSequence _sequence;
        private readonly SnapshotService _snapshotService;

        private readonly Stack<HistorySnapshot> _snapshots = new();

        public int SnapshotCount => _snapshots.Count;

        public HistoryService(CommandSequence sequence, SnapshotService snapshotService)
        {
            _sequence = sequence;
            _snapshotService = snapshotService;

            _sequence.BeforeCommandExecuteEvent += OnBeforeCommandExecute;
        }

        public void UndoLast()
        {
            if (_snapshots.Count > 0)
            {
                HistorySnapshot snapshot = _snapshots.Pop();

                _snapshotService.RestoreSnapshot(snapshot.States);
                _sequence.SetIndex(snapshot.CommandIndex);
            }
        }

        public void Clear()
        {
            _snapshots.Clear();
        }

        public void Dispose()
        {
            _sequence.BeforeCommandExecuteEvent -= OnBeforeCommandExecute;
            _snapshots.Clear();
        }

        private void OnBeforeCommandExecute(ICommand command, int commandIndex)
        {   
            Dictionary<ISnapshotable, object> states = _snapshotService.TakeSnapshot(command.SnapshotLayer);
            _snapshots.Push(new HistorySnapshot(states, commandIndex));
        }
    }
}