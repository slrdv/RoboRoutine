using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace RoboRoutine
{
    public sealed class HistoryService : IInitializable, IStartable, IDisposable
    {
        private readonly CommandSequence _sequence;
        private readonly SnapshotService _snapshotService;

        private readonly Stack<HistorySnapshot> _snapshots = new();
        private HistorySnapshot _initialSnapshot;

        public int SnapshotCount => _snapshots.Count;

        public HistoryService(CommandSequence sequence, SnapshotService snapshotService)
        {
            _sequence = sequence;
            _snapshotService = snapshotService;
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
            _sequence.BeforeCommandExecuteEvent += OnBeforeCommandExecute;
            _sequence.SequenceUpdatedEvent += Clear;
        }

        public void Start()
        {
            _initialSnapshot = new HistorySnapshot(_snapshotService.TakeSnapshot(SnapshotLayer.All), 0);
        }

        public void Dispose()
        {
            _sequence.SequenceUpdatedEvent -= Clear;
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