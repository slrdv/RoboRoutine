using UnityEngine;

namespace RoboRoutine
{
    public sealed class PutSystem
    {
        private readonly RobotController _robot;
        private readonly GridController _grid;
        private readonly ILevelCompleteListener _levelCompleteListener;

        public PutSystem(RobotController robot, GridController grid, ILevelCompleteListener levelCompleteListener)
        {
            _robot = robot;
            _grid = grid;
            _levelCompleteListener = levelCompleteListener;
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

                _levelCompleteListener.OnLevelComplete(LevelResult.Success);

                return CommandResult.Success;
            }

            return CommandResult.Failed;
        }
    }
}
