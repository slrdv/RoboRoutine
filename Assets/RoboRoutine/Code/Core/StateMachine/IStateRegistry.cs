namespace RoboRoutine.Core
{
    public interface IStateRegistry
    {
        T Get<T>() where T : class, IState;
    }
}
