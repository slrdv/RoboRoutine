using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public interface ICommandEditViewSetup
    {
        CommandType CommandType { get; }

        void Setup(CommandEditPanelView view, CommandData data);
        CommandData Apply();
    }
}
