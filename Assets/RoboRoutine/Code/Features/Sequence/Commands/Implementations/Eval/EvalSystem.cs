
using UnityEngine;

namespace RoboRoutine
{
    public sealed class EvalSystem
    {
        private readonly RobotController _robot;
        private readonly GridController _grid;

        public EvalSystem(RobotController robot, GridController grid)
        {
            _robot = robot;
            _grid = grid;
        }

        public CommandResult Eval(EvalType evalType)
        {
            if (_robot.Operand == null)
            {
                Debug.Log("Robot don't have operand");
                return CommandResult.Failed;
            }
            if (!_grid.TryFindAtWorldPositionXZ(_robot.GetPositionXZ(), out IGridEntityController item) || item is not OperandEntityController other)
            {
                Debug.Log("Target is not operand");
                return CommandResult.Failed;
            }
            if (!TryEvaluate(evalType, _robot.Operand.Value, other.Value, out int result))
            {
                Debug.Log($"Invalid evaluation type: {evalType}");
                return CommandResult.Failed;
            }

            _robot.Operand.SetValue(result);
            return CommandResult.Success;
        }

        private bool TryEvaluate(EvalType evalType, int l, int r, out int result)
        {
            switch (evalType)
            {
                case EvalType.Add:
                    result = l + r;
                    break;
                case EvalType.Subtract:
                    result = l - r;
                    break;
                default:
                    result = default;
                    return false;
            }

            return true;
        }
    }
}
