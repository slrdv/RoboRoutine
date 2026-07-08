using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class ConditionCommand : ICommand
    {
        public CommandType CommandType => CommandType.Condition;
        public SnapshotLayer SnapshotLayer => SnapshotLayer.None;

        private readonly ConditionCommandSystem _conditionSystem;
        private readonly ConditionType _conditionType;
        private readonly int _value;
        private readonly int _targetIndex;

        public ConditionCommand(ConditionCommandSystem system, ConditionType conditionType, int value, int targetIndex)
        {
            _conditionSystem = system;
            _conditionType = conditionType;
            _value = value;
            _targetIndex = targetIndex;
        }

        public UniTask<CommandResult> ExecuteAsync()
        {
            return UniTask.FromResult(_conditionSystem.Eval(_conditionType, _value, _targetIndex));
        }

        public void Cancel() { }

    }
}
