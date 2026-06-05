namespace RoboRoutine
{
    public interface ISnapshotable
    {
        SnapshotLayer SnapshotLayer { get; }
        object CaptureState();
        void RestoreState(object state);
    }
}