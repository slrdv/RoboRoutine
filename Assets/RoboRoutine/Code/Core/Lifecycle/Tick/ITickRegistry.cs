namespace RoboRoutine
{
    public interface ITickRegistry
    {
        void Add(ITickListener tickListener);
        void Remove(ITickListener tickListener);
    }
}