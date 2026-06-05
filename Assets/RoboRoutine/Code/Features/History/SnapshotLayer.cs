using System;

namespace RoboRoutine
{
    [Flags]
    public enum SnapshotLayer : uint
    {
        None = 1u << 0,
        Robot = 1u << 1,

        All = ~0u
    }
}