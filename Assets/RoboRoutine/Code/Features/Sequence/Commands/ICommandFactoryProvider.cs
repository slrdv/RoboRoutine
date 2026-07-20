namespace RoboRoutine.Features
{
    public interface ICommandFactoryProvider
    {
        ICommand Create(CommandData commandData);
    }
}
