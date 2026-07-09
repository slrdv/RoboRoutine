using System;

namespace RoboRoutine
{
    public sealed class SequenceBuilder : IDisposable
    {
        private readonly ICommandSequence _sequence;
        private readonly CommandListModel _listModel;
        private readonly ICommandFactoryProvider _commandFactory;

        public SequenceBuilder(ICommandSequence sequence, CommandListModel commandListModel, ICommandFactoryProvider commandFactory)
        {
            _sequence = sequence;
            _listModel = commandListModel;
            _commandFactory = commandFactory;
            _listModel.ChangedEvent += Build;
        }

        public void Build()
        {
            _sequence.RequestCancel();
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
