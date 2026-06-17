using System.Collections.Generic;

namespace RoboRoutine
{
    public interface IRepository<TKey, TItem>
    {
        int Count { get; }

        TItem Get(TKey key);
        bool TryGet(TKey key, out TItem item);
        IReadOnlyCollection<TItem> GetAll();
        void Load();
    }
}
