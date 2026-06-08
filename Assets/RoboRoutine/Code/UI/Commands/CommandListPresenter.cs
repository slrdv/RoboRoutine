using R3;
using ObservableCollections;
using VContainer.Unity;
using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandListPresenter : IInitializable, IDisposable
    {
        private readonly CommandListModel _model;
        private readonly CommandListView _view;
        private readonly CommandItemFactory _itemFactory;

        private readonly Dictionary<CommandItemModel, CommandItemPresenter> _items = new();
        private readonly CompositeDisposable _subscriptions = new();

        public CommandListPresenter(CommandListModel model, CommandListView view, CommandItemFactory itemFactory)
        {
            _model = model;
            _view = view;
            _itemFactory = itemFactory;
        }

        public void Initialize()
        {
            _view.Initialize();

            _model.Commands.ObserveAdd().Subscribe(OnItemAdded).AddTo(_subscriptions);
            _model.Commands.ObserveRemove().Subscribe(OnItemRemoved).AddTo(_subscriptions);
            _model.Commands.ObserveMove().Subscribe(OnItemMoved).AddTo(_subscriptions);
            _model.Commands.ObserveClear().Subscribe(OnItemsClear).AddTo(_subscriptions);

            foreach (CommandItemModel model in _model.Commands)
            {
                CreateItem(model);
            }

            _view.ItemDraggedEvent += OnItemDragged;
        }

        public void Dispose()
        {
            _view.ItemDraggedEvent -= OnItemDragged;
            _subscriptions.Dispose();
            ClearItems();
        }

        private void OnItemAdded(CollectionAddEvent<CommandItemModel> evt)
        {
            CreateItem(evt.Value);
        }

        private void OnItemRemoved(CollectionRemoveEvent<CommandItemModel> evt)
        {
            CommandItemModel model = evt.Value;
            CommandItemPresenter presenter = _items[model];

            _items.Remove(model);
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

        private void CreateItem(CommandItemModel model)
        {
            CommandItemPresenter presenter = _itemFactory.Create(model);
            _view.AddItem(presenter.View);
            _items.Add(model, presenter);
        }

        private void OnItemDragged(int from, int to)
        {
            _model.Move(from, to);
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

    }
}