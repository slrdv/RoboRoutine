namespace RoboRoutine
{
    public interface ICommandEditViewSetupFactory
    {
        void Setup(CommandEditPanelView view, CommandData data);
        CommandData Apply(CommandType type);
    }
}