using System;
using Newtonsoft.Json;

namespace RoboRoutine
{
    [Serializable]
    public class CommandData
    {
        public CommandType CommandType { get; private set; }

        [JsonConstructor]
        public CommandData([JsonProperty("CommandType")] CommandType type)
        {
            CommandType = type;
        }

        public virtual CommandData Clone()
        {
            return new CommandData(CommandType);
        }
    }
}