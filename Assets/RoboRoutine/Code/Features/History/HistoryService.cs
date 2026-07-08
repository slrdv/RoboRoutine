using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class HistoryService : IHistoryService, IDisposable
    {
        private readonly ICommandSequence _sequence;
        private readonly ICommandSequenceState _sequenceState;
        private readonly ISnapshotService _snapshotService;

        private readonly Stack<HistorySnapshot> _snapshots = new();
        private HistorySnapshot _initialSnapshot;

        public int SnapshotCount => _snapshots.Count;

        public HistoryService(ICommandSequence sequence, ICommandSequenceState sequenceState, ISnapshotService snapshotService)
        {
            _sequence = sequence;
            _sequenceState = sequenceState;
            _snapshotService = snapshotService;

            _sequenceState.BeforeCommandExecuteEvent += OnBeforeCommandExecute;
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

        public void RollbackToInitial()
        {
            _snapshots.Clear();
            _snapshotService.RestoreSnapshot(_initialSnapshot.States);
            _sequence.SetIndex(_initialSnapshot.CommandIndex);
        }

        public void Clear()
        {
            _snapshots.Clear();
        }

        public void Initialize()
        {
            _initialSnapshot = new HistorySnapshot(_snapshotService.TakeSnapshot(SnapshotLayer.All), 0);
        }

        public void Dispose()
        {
            _sequenceState.BeforeCommandExecuteEvent -= OnBeforeCommandExecute;
            _snapshots.Clear();
        }

        private void OnBeforeCommandExecute(ICommand command, int commandIndex)
        {
            Dictionary<ISnapshotable, object> states = _snapshotService.TakeSnapshot(command.SnapshotLayer);
            _snapshots.Push(new HistorySnapshot(states, commandIndex));
        }
    }
}
