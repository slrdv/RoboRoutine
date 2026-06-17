namespace RoboRoutine
{
    public sealed class MoveCommandFactory : ICommandFactory
    {
        private readonly MovementSystem _movementSystem;

        public CommandType CommandType => CommandType.Move;

        public MoveCommandFactory(MovementSystem movementSystem)
        {
            _movementSystem = movementSystem;
        }

        public ICommand Create(CommandData commandData)
        {
            MoveCommandData data = (MoveCommandData) commandData;
            return new MoveCommand(_movementSystem, data.MoveDirection, data.Distance);
        }
    }
}