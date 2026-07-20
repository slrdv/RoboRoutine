namespace RoboRoutine.Features
{
    public interface IHistoryService
    {
        int SnapshotCount { get; }

        void UndoLast();
        void RollbackToInitial();
        void Clear();
        void Initialize();
    }
}
