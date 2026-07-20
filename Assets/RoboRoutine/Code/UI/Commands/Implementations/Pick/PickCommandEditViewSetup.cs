using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class PickCommandEditViewSetup : DefaultEditViewSetup
    {
        public override CommandType CommandType => CommandType.Pick;
    }
}
