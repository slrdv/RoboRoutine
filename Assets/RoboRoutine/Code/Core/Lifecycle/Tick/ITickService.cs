namespace RoboRoutine.Core
{
    public interface ITickService
    {
        void SetTimeScale(float timeScale);
        void Start();
        void Stop();
    }
}
