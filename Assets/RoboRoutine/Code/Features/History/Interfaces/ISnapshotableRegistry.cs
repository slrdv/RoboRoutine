namespace RoboRoutine
{
    public interface ISnapshotableRegistry
    {
        void Register(ISnapshotable snapshotable);
        void Remove(ISnapshotable snapshotable);
    }
}
