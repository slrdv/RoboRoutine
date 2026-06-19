using R3;

namespace RoboRoutine
{
    public sealed class CommandItemModel
    {
        public ReadOnlyReactiveProperty<int> Index => _index;

        private readonly ReactiveProperty<int> _index = new();

        private readonly CommandData _commandData;

        public CommandData CommandData => _commandData;

        public CommandItemModel(CommandData commandData)
        {
            _commandData = commandData;
        }

        public void SetIndex(int index)
        {
            _index.Value = index;
        }
    }
}