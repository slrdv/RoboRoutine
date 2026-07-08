namespace RoboRoutine
{
    public interface ICommandFactoryProvider
    {
        ICommand Create(CommandData commandData);
    }
}
