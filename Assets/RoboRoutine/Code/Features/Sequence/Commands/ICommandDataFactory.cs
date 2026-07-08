namespace RoboRoutine
{
    public interface ICommandDataFactory
    {
        CommandData CreateDefaultCommandData(CommandType commandType);
    }
}
