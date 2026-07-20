using System;

namespace RoboRoutine.Features
{
    public interface ISimulationState
    {
        event Action ResetEvent;
        event Action StartEvent;
        event Action StopEvent;
        event Action FailedEvent;

        bool IsRunning { get; }
        int CommandIndex { get; }
        int CommandsCount { get; }
        bool IsInitialState { get; }
    }
}
