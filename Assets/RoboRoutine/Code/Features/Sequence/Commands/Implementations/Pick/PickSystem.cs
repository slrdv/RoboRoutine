namespace RoboRoutine
{
    public sealed class PickSystem
    {
        private readonly RobotController _robot;
        private readonly GridController _grid;

        public PickSystem(RobotController robot, GridController grid)
        {
            _robot = robot;
            _grid = grid;
        }

        public CommandResult Pick()
        {
            if (_grid.TryGetAtWorldPositionXZ(_robot.GetPositionXZ(), out IGridEntityController item))
            {
                if (item is OperandEntityController operand)
                {
                    _grid.RemoveEntity(operand);
                    _robot.PickOperand(operand);
                    return CommandResult.Success;
                }
            }
            return CommandResult.Failed;
        }
    }
}