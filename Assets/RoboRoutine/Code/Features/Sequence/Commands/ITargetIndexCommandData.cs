namespace RoboRoutine.Features
{
    public interface ITargetIndexCommandData
    {
        int TargetIndex { get; }

        CommandData WithTargetIndex(int targetIndex);
    }
}
