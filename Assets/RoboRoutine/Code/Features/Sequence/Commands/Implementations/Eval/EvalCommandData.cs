using System;
using Newtonsoft.Json;

namespace RoboRoutine.Features
{
    [Serializable]
    public sealed class EvalCommandData : CommandData
    {
        public EvalType EvalType { get; private set; }

        [JsonConstructor]
        public EvalCommandData([JsonProperty("EvalType")] EvalType evalType) : base(CommandType.Eval)
        {
            EvalType = evalType;
        }

        public override CommandData Clone()
        {
            return new EvalCommandData(EvalType);
        }
    }
}
