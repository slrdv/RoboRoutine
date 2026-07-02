namespace RoboRoutine
{
    public interface ISimulationService
    {
        void RunAll();
        void RunNext();
        void Back();
        void Stop();
        bool CanRunNext();
        bool CanBack();
        bool CanStop();
    }
}