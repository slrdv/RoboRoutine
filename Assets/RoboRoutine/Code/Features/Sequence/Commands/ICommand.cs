using System;

namespace RoboRoutine
{
    public interface ICommand
    {
        event Action<CommandResult> OnComplete;

        CommandType CommandType { get; }

        void Execute();
        void Cancel();
    }
}