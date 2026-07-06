using UnityEngine;

namespace RoboRoutine
{
    public sealed class PutSystem
    {
        private readonly RobotController _robot;
        private readonly GridController _grid;

        public PutSystem(RobotController robot, GridController grid)
        {
            _robot = robot;
            _grid = grid;
        }

        public CommandResult Put()
        {
            Vector2Int cell = _grid.WorldToCell(_robot.GetPositionXZ());

            if (_robot.Operand != null && _grid.TryAddEntity(_robot.RemoveOperand(), cell))
            {
                return CommandResult.Success;
            }
            return CommandResult.Failed;
        }
    }
}