using System;

namespace RoboRoutine
{
    [Serializable]
    public struct SerializablePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
}
