namespace RoboRoutine.Features
{
    public interface ICommandDataFactory
    {
        CommandData CreateDefaultCommandData(CommandType commandType);
    }
}
