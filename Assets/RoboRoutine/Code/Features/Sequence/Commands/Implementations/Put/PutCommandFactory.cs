namespace RoboRoutine
{
    public sealed class PutCommandFactory : ICommandFactory
    {
        private readonly PutSystem _putSystem;

        public CommandType CommandType => CommandType.Put;

        public PutCommandFactory(PutSystem putSystem)
        {
            _putSystem = putSystem;
        }

        public ICommand Create(CommandData commandData)
        {
            return new PutCommand(_putSystem);
        }

        public CommandData CreateDefaultData()
        {
            return new CommandData(CommandType.Put);
        }
    }
}