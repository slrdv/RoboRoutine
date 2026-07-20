using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class PutCommandEditViewSetup : DefaultEditViewSetup
    {
        public override CommandType CommandType => CommandType.Put;
    }
}
