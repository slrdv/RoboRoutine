using System;

namespace RoboRoutine
{
    public interface ISimulationState
    {
        event Action SimulationRunEvent;
        event Action SimulationStopEvent;

        bool IsRunning { get; }
        int CommandIndex { get; }
        int CommandsCount { get; }
        bool IsInitialState { get; }
    }
}