using System;

namespace RoboRoutine
{
    [Flags]
    public enum SnapshotLayer : uint
    {
        None = 0u,
        Robot = 1u << 0,
        Grid = 1u << 1,
        Operand = 1u << 2,
        Slot = 1u << 3,
        All = ~0u
    }
}