using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class PickCommand : ICommand
    {
        public CommandType CommandType => CommandType.Pick;
        public SnapshotLayer SnapshotLayer => SnapshotLayer.Robot | SnapshotLayer.Grid;

        private readonly PickSystem _pickSystem;

        public PickCommand(PickSystem pickSystem)
        {
            _pickSystem = pickSystem;
        }

        public async UniTask<CommandResult> ExecuteAsync()
        {
            return _pickSystem.Pick();
        }

        public void Cancel() { }

    }
}