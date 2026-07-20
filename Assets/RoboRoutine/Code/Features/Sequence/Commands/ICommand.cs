using Cysharp.Threading.Tasks;

namespace RoboRoutine.Features
{
    public interface ICommand
    {
        CommandType CommandType { get; }
        SnapshotLayer SnapshotLayer { get; }

        UniTask<CommandResult> ExecuteAsync();
        void Cancel();
    }
}
