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

        public CommandData CreateDefaultData()
        {
            return new MoveCommandData { Type = CommandType.Move, MoveDirection = MoveDirection.Up, Distance = 1 };
        }

        public CommandData CloneData(CommandData commandData)
        {
            MoveCommandData data = (MoveCommandData)commandData;
            return new MoveCommandData { Type = data.Type, MoveDirection = data.MoveDirection, Distance = data.Distance };
        }
    }
}