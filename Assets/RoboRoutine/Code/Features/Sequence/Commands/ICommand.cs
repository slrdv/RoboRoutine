using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public interface ICommand
    {
        CommandType CommandType { get; }

        UniTask<CommandResult> ExecuteAsync();
        void Cancel();
    }
}