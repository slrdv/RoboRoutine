using System;
using Newtonsoft.Json;

namespace RoboRoutine.Features
{
    [Serializable]
    public sealed class MoveCommandData : CommandData
    {
        public MoveDirection MoveDirection { get; private set; }
        public int Distance { get; private set; }

        [JsonConstructor]
        public MoveCommandData([JsonProperty("MoveDirection")] MoveDirection direction, [JsonProperty("Distance")] int distance) : base(CommandType.Move)
        {
            MoveDirection = direction;
            Distance = distance;
        }

        public override CommandData Clone()
        {
            return new MoveCommandData(MoveDirection, Distance);
        }
    }
}
