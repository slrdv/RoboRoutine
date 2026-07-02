using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class SnapshotService : ISnapshotService, ISnapshotableRegistry, IDisposable
    {
        private readonly List<ISnapshotable> _snapshotables = new();

        public void Register(ISnapshotable snapshotable)
        {
            if (_snapshotables.Contains(snapshotable)) return;

            _snapshotables.Add(snapshotable);
        }

        public void Remove(ISnapshotable snapshotable)
        {
            _snapshotables.Remove(snapshotable);
        }

        public Dictionary<ISnapshotable, object> TakeSnapshot(SnapshotLayer layer)
        {
            Dictionary<ISnapshotable, object> states = new();

            for (int i = 0; i < _snapshotables.Count; i++)
            {
                ISnapshotable snapshotable = _snapshotables[i];
                if ((snapshotable.SnapshotLayer & layer) != 0)
                {
                    states[snapshotable] = snapshotable.CaptureState();
                }
            }

            return states;
        }

        public void RestoreSnapshot(Dictionary<ISnapshotable, object> snapshot)
        {
            foreach (var (snapshotable, state) in snapshot)
            {
                snapshotable.RestoreState(state);
            }
        }

        public void Dispose()
        {
            _snapshotables.Clear();
        }
    }
}