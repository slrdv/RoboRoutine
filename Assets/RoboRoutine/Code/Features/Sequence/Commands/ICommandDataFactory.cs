namespace RoboRoutine
{
    public interface ICommandDataFactory
    {
        CommandData CreateDefaultCommandData(CommandType commandType);
        CommandData CloneData(CommandData commandData);
    }
}