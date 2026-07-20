namespace RoboRoutine.Features
{
    public interface ISimulationService
    {
        void RunAll();
        void RunNext();
        void Undo();
        void Reset();
        bool CanRunNext();
        bool CanUndo();
        bool CanReset();
    }
}
