using ObservableCollections;

namespace RoboRoutine
{
    public sealed class CommandListModel
    {
        private readonly ObservableList<CommandItemModel> _commands = new();

        public IReadOnlyObservableList<CommandItemModel> Commands => _commands;

        public void Add(CommandItemModel command)
        {
            _commands.Add(command);
            command.SetIndex(_commands.Count - 1);
        }

        public void Remove(CommandItemModel command)
        {
            _commands.Remove(command);
            UpdateIndexes();
        }

        public void Move(int fromIndex, int toIndex)
        {
            _commands.Move(fromIndex, toIndex);
            UpdateIndexes();
        }

        public void Insert(CommandItemModel command, int index)
        {
            _commands.Insert(index, command);
            UpdateIndexes();
        }

        public void Clear()
        {
            _commands.Clear();
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                _commands[i].SetIndex(i);
            }
        }
    }
}