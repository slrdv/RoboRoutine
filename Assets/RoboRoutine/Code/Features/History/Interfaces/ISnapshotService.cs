using System.Collections.Generic;

namespace RoboRoutine
{
    public interface ISnapshotService
    {
        Dictionary<ISnapshotable, object> TakeSnapshot(SnapshotLayer layer);
        void RestoreSnapshot(Dictionary<ISnapshotable, object> snapshot);
    }
}
