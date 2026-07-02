namespace RoboRoutine
{
    public interface ICommandEditViewSetupProvider
    {
        void Setup(CommandEditPanelView view, CommandData data);
        CommandData Apply(CommandType type);
    }
}