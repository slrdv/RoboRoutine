using System;

namespace RoboRoutine
{
    public sealed class SequenceBuilder : IDisposable
    {
        private readonly CommandSequence _sequence;
        private readonly CommandListModel _listModel;
        private readonly CommandFactory _commandFactory;

        public SequenceBuilder(CommandSequence sequence, CommandListModel commandListModel, CommandFactory commandFactory)
        {
            _sequence = sequence;
            _listModel = commandListModel;
            _commandFactory = commandFactory;
            _listModel.ChangedEvent += Build;
        }

        public void Build()
        {
            _sequence.Cancel();
            _sequence.Clear();

            for (int i = 0; i < _listModel.Items.Count; i++)
            {
                CommandData data = _listModel.Items[i].CommandData;
                _sequence.Add(_commandFactory.Create(data));
            }
        }

        public void Dispose()
        {
            _listModel.ChangedEvent -= Build;
        }
    }
}