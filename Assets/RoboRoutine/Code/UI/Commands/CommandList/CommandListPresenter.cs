using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandListPresenter : IDisposable
    {
        public event Action<CommandItemModel> ItemClickEvent;
        private readonly CommandListModel _model;
        private readonly CommandListView _view;
        private readonly ICommandItemFactory _itemFactory;
        private readonly ISimulationState _simulationState;
        private readonly ICommandCrossPanelDragEventProvider _externalDragEventProvider;
        private readonly Dictionary<CommandItemModel, CommandItemPresenter> _items = new();

        public CommandListPresenter(
            CommandListModel model,
            CommandListView view,
            ICommandItemFactory itemFactory,
            ISimulationState simulationState,
            ICommandCrossPanelDragEventProvider dragEventProvider)
        {
            _model = model;
            _view = view;
            _itemFactory = itemFactory;
            _simulationState = simulationState;
            _externalDragEventProvider = dragEventProvider;

            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
            OnItemsClear();
        }

        private void OnItemAdded(int index, CommandItemModel item)
        {
            CreateItem(item, index);
        }

        private void OnItemRemoved(int index, CommandItemModel item)
        {
            CommandItemPresenter presenter = _items[item];

            presenter.ClickEvent -= OnItemClicked;
            _items.Remove(item);
            _view.RemoveItem(index);
            presenter.Dispose();
        }

        private void OnItemMoved(int from, int to)
        {
            _view.MoveItem(from, to);
        }

        private void OnItemsClear()
        {
            _view.ClearItems();
            foreach (var presenter in _items.Values)
            {
                presenter.ClickEvent -= OnItemClicked;
                presenter.Dispose();
            }
            _items.Clear();
        }

        private void CreateItem(CommandItemModel model, int index)
        {
            CommandItemPresenter presenter = _itemFactory.Create(model);
            _view.AddItem(presenter.View, index);
            _items.Add(model, presenter);
            presenter.ClickEvent += OnItemClicked;
        }

        private void OnItemDropped(int from, int to)
        {
            _model.Move(from, to);
        }

        private void OnExternalItemDropped(int index, CommandData commandData)
        {
            _model.Insert(new CommandItemModel(commandData.Clone()), index);
        }

        private void OnItemDroppedOut(int index)
        {
            _model.Remove(index);
        }

        private void UpdateUI()
        {
            if (_simulationState.IsInitialState)
            {
                _view.HidePointer();
                return;
            }

            if (_simulationState.CommandIndex < _simulationState.CommandsCount)
            {
                _view.ShowPointer(_simulationState.CommandIndex);
            }
        }

        private void OnItemClicked(CommandItemPresenter item)
        {
            ItemClickEvent?.Invoke(GetModel(item));
        }

        private CommandItemModel GetModel(CommandItemPresenter presenter)
        {
            foreach (KeyValuePair<CommandItemModel, CommandItemPresenter> kv in _items)
            {
                if (presenter == kv.Value)
                {
                    return kv.Key;
                }
            }

            throw new KeyNotFoundException("Model not found");
        }

        private void Subscribe()
        {
            _model.ItemAddedEvent += OnItemAdded;
            _model.ItemRemovedEvent += OnItemRemoved;
            _model.ItemMovedEvent += OnItemMoved;
            _model.ClearedEvent += OnItemsClear;

            _simulationState.SimulationRunEvent += UpdateUI;
            _simulationState.SimulationStopEvent += UpdateUI;

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

            _simulationState.SimulationStopEvent -= UpdateUI;
            _simulationState.SimulationRunEvent -= UpdateUI;

            _model.ItemAddedEvent -= OnItemAdded;
            _model.ItemRemovedEvent -= OnItemRemoved;
            _model.ItemMovedEvent -= OnItemMoved;
            _model.ClearedEvent -= OnItemsClear;
        }
    }
}