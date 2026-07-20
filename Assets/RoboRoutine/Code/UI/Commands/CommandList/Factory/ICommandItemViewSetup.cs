using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public interface ICommandItemViewSetup
    {
        CommandType CommandType { get; }
        void Setup(CommandItemView view, CommandConfig config, CommandData commandData);
    }
}
