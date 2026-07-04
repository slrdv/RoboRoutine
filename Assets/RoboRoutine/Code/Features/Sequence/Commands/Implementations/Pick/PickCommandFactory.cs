namespace RoboRoutine
{
    public sealed class PickCommandFactory : ICommandFactory
    {
        private readonly PickSystem _pickSystem;

        public CommandType CommandType => CommandType.Pick;

        public PickCommandFactory(PickSystem pickSystem)
        {
            _pickSystem = pickSystem;
        }

        public ICommand Create(CommandData commandData)
        {
            return new PickCommand(_pickSystem);
        }

        public CommandData CreateDefaultData()
        {
            return new CommandData(CommandType.Pick);
        }
    }
}