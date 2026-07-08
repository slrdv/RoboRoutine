using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public interface ICommandSequenceState
    {
        event Action<ICommand, int> BeforeCommandExecuteEvent;
        event Action<CommandResult, int> CommandCompleteEvent;
        event Action SequenceUpdatedEvent;

        IReadOnlyList<ICommand> Commands { get; }
        int CommandIndex { get; }
        int Count { get; }
        bool IsExecuting { get; }
    }
}
