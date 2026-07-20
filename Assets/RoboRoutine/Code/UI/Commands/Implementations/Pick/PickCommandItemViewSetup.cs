using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class PickCommandItemViewSetup : DefaultCommandItemViewSetup
    {
        public override CommandType CommandType => CommandType.Pick;
    }
}
