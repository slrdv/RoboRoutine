using System;

namespace RoboRoutine
{
    [Serializable]
    public abstract class CommandData
    {
        public CommandType Type { get; private set; }

        public CommandData(CommandType type)
        {
            Type = type;
        }

        public abstract CommandData Clone();
    }
}