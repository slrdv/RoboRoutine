using System;
using Newtonsoft.Json;

namespace RoboRoutine
{
    [Serializable]
    public sealed class ConditionCommandData : CommandData, ITargetIndexCommandData
    {
        public ConditionType ConditionType { get; private set; }
        public int Value { get; private set; }
        public int TargetIndex { get; private set; }

        [JsonConstructor]
        public ConditionCommandData(
            [JsonProperty("ConditionType")] ConditionType conditionType,
            [JsonProperty("Value")] int value,
            [JsonProperty("TargetIndex")] int targetIndex) : base(CommandType.Condition)
        {
            ConditionType = conditionType;
            Value = value;
            TargetIndex = targetIndex;
        }

        public override CommandData Clone()
        {
            return new ConditionCommandData(ConditionType, Value, TargetIndex);
        }

        public CommandData WithTargetIndex(int targetIndex)
        {
            return new ConditionCommandData(ConditionType, Value, targetIndex);
        }
    }
}