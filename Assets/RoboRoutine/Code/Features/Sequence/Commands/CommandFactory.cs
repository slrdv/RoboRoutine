namespace RoboRoutine
{
    public sealed class CommandFactory
    {
        private readonly MovementSystem _movementSystem;

        public CommandFactory(MovementSystem movementSystem)
        {
            _movementSystem = movementSystem;
        }

        public MoveCommand CreateMoveCommand(MoveDirection direction, int distance)
        {
            return new MoveCommand(_movementSystem, direction, distance);
        }
    }
}