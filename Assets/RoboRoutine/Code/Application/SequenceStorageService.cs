using System;
using ObservableCollections;
using R3;

namespace RoboRoutine
{
    public sealed class SequenceStorageService : IDisposable
    {
        private readonly CommandListModel _commandlistModel;
        private readonly IStorage<SequenceData> _storage;

        private readonly CompositeDisposable _subscriptions = new();
        private bool _isLoading;


        public SequenceStorageService(CommandListModel commandListModel, IStorage<SequenceData> storage)
        {
            _commandlistModel = commandListModel;
            _storage = storage;

            _commandlistModel.Commands.ObserveChanged().Subscribe(_ => Save()).AddTo(_subscriptions);
        }

        public void Load()
        {
            SequenceData sequenceData = _storage.Load();

            if (sequenceData == null || sequenceData.Commands == null) return;

            _isLoading = true;

            _commandlistModel.Clear();

            for (int i = 0; i < sequenceData.Commands.Count; i++)
            {
                CommandData data = sequenceData.Commands[i];
                _commandlistModel.Add(new CommandItemModel(data));
            }

            _isLoading = false;
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

        private void Save()
        {
            if (_isLoading) return;
            
            SequenceData sequenceData = new();
            sequenceData.Commands = new(_commandlistModel.Commands.Count);

            for (int i = 0; i < _commandlistModel.Commands.Count; i++)
            {
                CommandData data = _commandlistModel.Commands[i].CommandData;
                sequenceData.Commands.Add(data);
            }

            _storage.Save(sequenceData);
        }
    }
}