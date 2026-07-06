using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class PutCommand : ICommand
    {
        public CommandType CommandType => CommandType.Put;
        public SnapshotLayer SnapshotLayer => SnapshotLayer.Robot | SnapshotLayer.Grid;

        private readonly PutSystem _putSystem;

        public PutCommand(PutSystem putSystem)
        {
            _putSystem = putSystem;
        }

        public async UniTask<CommandResult> ExecuteAsync()
        {
            return _putSystem.Put();
        }

        public void Cancel() { }

    }
}