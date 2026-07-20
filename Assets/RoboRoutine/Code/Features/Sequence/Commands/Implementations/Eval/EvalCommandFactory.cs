namespace RoboRoutine.Features
{
    public sealed class EvalCommandFactory : ICommandFactory
    {
        private readonly EvalSystem _system;

        public CommandType CommandType => CommandType.Eval;

        public EvalCommandFactory(EvalSystem system)
        {
            _system = system;
        }

        public ICommand Create(CommandData commandData)
        {
            EvalCommandData data = (EvalCommandData)commandData;
            return new EvalCommand(_system, data.EvalType);
        }

        public CommandData CreateDefaultData()
        {
            return new EvalCommandData(EvalType.None);
        }
    }
}
