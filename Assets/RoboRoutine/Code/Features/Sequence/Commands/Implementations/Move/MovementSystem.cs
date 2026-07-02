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

        public async UniTask<CommandResult> MoveAsync(MoveDirection moveDirection, int cellDistance, CancellationToken ct)
        {
            Vector2Int cell = _grid.WorldToCell(_robot.GetPositionXZ());
            Vector2Int direction = GetDirectionVector(moveDirection);

            for (int i = 0; i < cellDistance; i++)
            {
                cell += direction;
                Vector2 targetPosition = _grid.GetCellCenterWorldPositionXZ(cell);

                bool cancelled = await _robot.MoveXZAsync(targetPosition, ct).SuppressCancellationThrow();
                if (cancelled)
                {
                    return CommandResult.Canceled;
                }
            }

            return CommandResult.Success;
        }

        private Vector2Int GetDirectionVector(MoveDirection direction)
        {
            Vector2Int vector = new Vector2Int();
            switch (direction)
            {
                case MoveDirection.Up:
                    return Vector2Int.up;
                case MoveDirection.Down:
                    return Vector2Int.down;
                case MoveDirection.Left:
                    return Vector2Int.left;
                case MoveDirection.Right:
                    return Vector2Int.right;
            }

            return vector;
        }
    }
}