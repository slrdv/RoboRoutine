using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class MovementSystem
    {
        private readonly RobotController _robot;
        private readonly GridController _grid;

        public MovementSystem(RobotController robot, GridController grid)
        {
            _robot = robot;
            _grid = grid;
        }

        public async UniTask<CommandResult> MoveAsync(MoveDirection moveDirection, int cellDistance)
        {
            Vector2Int cell = _grid.WorldToCell(_robot.GetPositionXZ());
            Vector2Int direction = GetDirectionVector(moveDirection);

            for (int i = 0; i < cellDistance; i++)
            {
                cell += direction;
                Vector2 targetPosition = _grid.GetCellCenterWorldPositionXZ(cell);

                OperationResult result = await _robot.MoveXZAsync(targetPosition);
                if (result == OperationResult.Cancelled)
                {
                    return CommandResult.Canceled;
                }
            }

            return CommandResult.Success;
        }

        public void Stop()
        {
            _robot.StopCurrentOperation();
        }

        private Vector2Int GetDirectionVector(MoveDirection direction)
        {
            return direction switch
            {
                MoveDirection.Up => Vector2Int.up,
                MoveDirection.Down => Vector2Int.down,
                MoveDirection.Left => Vector2Int.left,
                MoveDirection.Right => Vector2Int.right,
                _ => Vector2Int.zero
            };
        }
    }
}