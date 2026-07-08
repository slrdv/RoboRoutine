namespace RoboRoutine
{
    public sealed class ConditionCommandSystem
    {
        private readonly RobotController _robot;
        private readonly ICommandSequence _sequence;

        public ConditionCommandSystem(RobotController robot, ICommandSequence sequence)
        {
            _robot = robot;
            _sequence = sequence;
        }

        public CommandResult Eval(ConditionType conditionType, int value, int targetIndex)
        {
            if (_robot.Operand == null || targetIndex < 0) return CommandResult.Failed;

            if (!TryEvaluate(conditionType, _robot.Operand.Value, value, out bool result)) return CommandResult.Failed;

            if (result)
            {
                _sequence.SetIndexAfterExecution(targetIndex);
            }

            return CommandResult.Success;
        }

        private bool TryEvaluate(ConditionType conditionType, int l, int r, out bool result)
        {
            switch (conditionType)
            {
                case ConditionType.Less:
                    result = l < r;
                    break;
                case ConditionType.LessOrEqual:
                    result = l <= r;
                    break;
                case ConditionType.Equal:
                    result = l == r;
                    break;
                case ConditionType.Greater:
                    result = l > r;
                    break;
                case ConditionType.GreaterOrEqual:
                    result = l >= r;
                    break;
                default:
                    result = default;
                    return false;
            }

            return true;
        }
    }
}
