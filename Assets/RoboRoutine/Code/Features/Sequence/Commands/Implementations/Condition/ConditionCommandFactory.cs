namespace RoboRoutine
{
    public sealed class ConditionCommandFactory : ICommandFactory
    {
        private readonly ConditionCommandSystem _system;

        public CommandType CommandType => CommandType.Condition;

        public ConditionCommandFactory(ConditionCommandSystem system)
        {
            _system = system;
        }

        public ICommand Create(CommandData commandData)
        {
            ConditionCommandData data = (ConditionCommandData)commandData;
            return new ConditionCommand(_system, data.ConditionType, data.Value, data.TargetIndex);
        }

        public CommandData CreateDefaultData()
        {
            return new ConditionCommandData(ConditionType.None, 0, -1);
        }
    }
}