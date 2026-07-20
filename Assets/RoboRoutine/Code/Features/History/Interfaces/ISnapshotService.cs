using System.Collections.Generic;

namespace RoboRoutine.Features
{
    public interface ISnapshotService
    {
        Dictionary<ISnapshotable, object> TakeSnapshot(SnapshotLayer layer);
        void RestoreSnapshot(Dictionary<ISnapshotable, object> snapshot);
    }
}
