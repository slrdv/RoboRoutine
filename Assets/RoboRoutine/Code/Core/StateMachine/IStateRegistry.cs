namespace RoboRoutine
{
    public interface IStateRegistry
    {
        T Get<T>() where T : class, IState;
    }
}
