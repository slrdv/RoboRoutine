namespace RoboRoutine
{
    public interface ICommandEditViewSetup
    {
        CommandType CommandType { get; }

        void Setup(CommandEditPanelView view, CommandData data);
        CommandData Apply();
    }
}
