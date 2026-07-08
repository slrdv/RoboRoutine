namespace RoboRoutine
{
    public interface IStorage<T>
    {
        T Load();
        void Save(T data);
        void Clear();
    }
}
