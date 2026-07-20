using System;
using R3;
using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class CommandItemModel
    {
        public event Action<CommandItemModel> DataChangedEvent;

        public ReadOnlyReactiveProperty<int> Index => _index;

        private readonly ReactiveProperty<int> _index = new();

        private CommandData _commandData;

        public CommandData CommandData => _commandData;

        public CommandItemModel(CommandData commandData)
        {
            _commandData = commandData;
        }

        public void SetIndex(int index)
        {
            _index.Value = index;
        }

        public void SetCommandData(CommandData commandData)
        {
            _commandData = commandData;
            DataChangedEvent?.Invoke(this);
        }
    }
}
