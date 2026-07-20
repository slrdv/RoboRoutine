namespace RoboRoutine.Features
{
    public interface ISnapshotable
    {
        SnapshotLayer SnapshotLayer { get; }
        object CaptureState();
        void RestoreState(object state);
    }
}
