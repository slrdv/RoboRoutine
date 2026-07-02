namespace RoboRoutine
{
    public interface ICommandFactory
    {
        CommandType CommandType { get; }
        ICommand Create(CommandData data);
        CommandData CreateDefaultData();
    }
}