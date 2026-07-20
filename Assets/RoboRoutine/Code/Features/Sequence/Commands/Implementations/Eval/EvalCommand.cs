using Cysharp.Threading.Tasks;

namespace RoboRoutine.Features
{
    public sealed class EvalCommand : ICommand
    {
        public CommandType CommandType => CommandType.Eval;
        public SnapshotLayer SnapshotLayer => SnapshotLayer.Operand;

        private readonly EvalSystem _evalSystem;
        private readonly EvalType _evalType;

        public EvalCommand(EvalSystem evalSystem, EvalType evalType)
        {
            _evalSystem = evalSystem;
            _evalType = evalType;
        }

        public UniTask<CommandResult> ExecuteAsync()
        {
            return UniTask.FromResult(_evalSystem.Eval(_evalType));
        }

        public void Cancel() { }

    }
}
