using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public interface ICommandEditViewSetupProvider
    {
        void Setup(CommandEditPanelView view, CommandData data);
        CommandData Apply(CommandType type);
    }
}
