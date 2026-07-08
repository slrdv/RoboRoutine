namespace RoboRoutine
{
    public interface ICommandItemViewSetup
    {
        CommandType CommandType { get; }
        void Setup(CommandItemView view, CommandConfig config, CommandData commandData);
    }
}
