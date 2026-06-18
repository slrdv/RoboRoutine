using System;
using ObservableCollections;
using R3;

namespace RoboRoutine
{
    public sealed class SequenceBuilder : IDisposable
    {
        private readonly CommandSequence _sequence;
        private readonly CommandListModel _listModel;
        private readonly CommandFactory _commandFactory;

        private readonly CompositeDisposable _subscriptions = new();

        public SequenceBuilder(CommandSequence sequence, CommandListModel commandListModel, CommandFactory commandFactory)
        {
            _sequence = sequence;
            _listModel = commandListModel;
            _commandFactory = commandFactory;

            _listModel.Commands.ObserveChanged().Subscribe(_ => Build()).AddTo(_subscriptions);
        }

        public void Build()
        {
            _sequence.Clear();

            for (int i = 0; i < _listModel.Commands.Count; i++)
            {
                CommandData data = _listModel.Commands[i].CommandData;
                _sequence.Add(_commandFactory.Create(data));
            }
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}