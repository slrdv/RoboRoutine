using R3;
using ObservableCollections;
using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandListPresenter : IDisposable
    {
        private readonly CommandListModel _model;
        private readonly CommandListView _view;
        private readonly CommandItemFactory _itemFactory;
        private readonly SimulationService _simulationService;
        private readonly ICommandCrossPanelDragEventProvider _externalDragEventProvider;
        private readonly ICommandDataFactory _dataFactory;
        private readonly Dictionary<CommandItemModel, CommandItemPresenter> _items = new();
        private readonly CompositeDisposable _subscriptions = new();

        public CommandListPresenter(
            CommandListModel model,
            CommandListView view,
            CommandItemFactory itemFactory,
            SimulationService simulationService,
            ICommandCrossPanelDragEventProvider dragEventProvider,
            ICommandDataFactory dataFactory)
        {
            _model = model;
            _view = view;
            _itemFactory = itemFactory;
            _simulationService = simulationService;
            _externalDragEventProvider = dragEventProvider;
            _dataFactory = dataFactory;

            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
            ClearItems();
        }

        private void OnItemAdded(CollectionAddEvent<CommandItemModel> evt)
        {
            CreateItem(evt.Value, evt.Index);
        }

        private void OnItemRemoved(CollectionRemoveEvent<CommandItemModel> evt)
        {
            CommandItemModel model = evt.Value;
            CommandItemPresenter presenter = _items[model];

            _items.Remove(model);
            _view.RemoveItem(evt.Index);
            presenter.Dispose();
        }

        private void OnItemMoved(CollectionMoveEvent<CommandItemModel> evt)
        {
            _view.MoveItem(evt.OldIndex, evt.NewIndex);
        }

        private void OnItemsClear(Unit _)
        {
            ClearItems();
        }

        private void CreateItem(CommandItemModel model, int index)
        {
            CommandItemPresenter presenter = _itemFactory.Create(model);
            _view.AddItem(presenter.View, index);
            _items.Add(model, presenter);
        }

        private void OnItemDropped(int from, int to)
        {
            _model.Move(from, to);
        }

        private void OnExternalItemDropped(int index, CommandData commandData)
        {
            _model.Insert(new CommandItemModel(_dataFactory.CloneData(commandData)), index);
        }

        private void OnItemDroppedOut(int index)
        {
            _model.Remove(index);
        }

        private void ClearItems()
        {
            _view.ClearItems();
            foreach (var presenter in _items.Values)
            {
                presenter.Dispose();
            }
            _items.Clear();
        }

        private void UpdateUI()
        {
            if (_simulationService.IsInitialState)
            {
                _view.HidePointer();
                _view.SetInputEnabled(true);
                return;
            }

            _view.SetInputEnabled(false);

            if (_simulationService.CommandIndex < _simulationService.CommandsCount)
            {
                _view.ShowPointer(_simulationService.CommandIndex);
            }
        }

        private void Subscribe()
        {
            _model.Commands.ObserveAdd().Subscribe(OnItemAdded).AddTo(_subscriptions);
            _model.Commands.ObserveRemove().Subscribe(OnItemRemoved).AddTo(_subscriptions);
            _model.Commands.ObserveMove().Subscribe(OnItemMoved).AddTo(_subscriptions);
            _model.Commands.ObserveClear().Subscribe(OnItemsClear).AddTo(_subscriptions);

            _simulationService.SimulationRunEvent += UpdateUI;
            _simulationService.SimulationStopEvent += UpdateUI;

            _view.ItemDroppedEvent += OnItemDropped;
            _view.ExternalItemDroppedEvent += OnExternalItemDropped;
            _view.ItemDroppedOutEvent += OnItemDroppedOut;

            _externalDragEventProvider.DragEvent += _view.OnExternalDrag;
            _externalDragEventProvider.EndDragEvent += _view.OnExternalEndDrag;
        }

        private void Unsubscribe()
        {
            _externalDragEventProvider.DragEvent -= _view.OnExternalDrag;
            _externalDragEventProvider.EndDragEvent -= _view.OnExternalEndDrag;

            _view.ExternalItemDroppedEvent -= OnExternalItemDropped;
            _view.ItemDroppedEvent -= OnItemDropped;
            _view.ItemDroppedOutEvent -= OnItemDroppedOut;

            _simulationService.SimulationStopEvent -= UpdateUI;
            _simulationService.SimulationRunEvent -= UpdateUI;

            _subscriptions.Dispose();
        }
    }
}