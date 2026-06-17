using System;

namespace RoboRoutine
{
    [Serializable]
    public sealed class MoveCommandData : CommandData
    {
        public MoveDirection MoveDirection;
        public int Distance;
    }
}