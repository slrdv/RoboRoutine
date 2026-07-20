using System.Collections.Generic;

namespace RoboRoutine.Features
{
    public sealed class HistorySnapshot
    {
        public readonly Dictionary<ISnapshotable, object> States;
        public readonly int CommandIndex;

        public HistorySnapshot(Dictionary<ISnapshotable, object> states, int commandIndex)
        {
            States = states;
            CommandIndex = commandIndex;
        }
    }
}
