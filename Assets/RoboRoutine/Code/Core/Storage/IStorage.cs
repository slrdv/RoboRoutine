namespace RoboRoutine
{
    public interface IStorage<TKey, T>
    {
        T Load();
        void Save(T data);
        void Clear();
        void Initialize(TKey key);
    }
}
