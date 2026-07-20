using System;

namespace RoboRoutine.Core
{
    [Serializable]
    public struct SerializablePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
}
