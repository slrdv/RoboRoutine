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
            if (_robot.Operand == null) return CommandResult.Failed;

            Vector2Int cell = _grid.WorldToCell(_robot.GetPositionXZ());

            if (!_grid.TryFindAtWorldPositionXZ(cell, out IGridEntityController item)) return CommandResult.Success;

            if (item is not SlotEntityController slot) return CommandResult.Failed;

            if (_robot.Operand.Value == slot.Value)
            {
                slot.SetActivated(true);
                Debug.Log("Win!");
                return CommandResult.Success;
            }

            return CommandResult.Failed;
        }
    }
}