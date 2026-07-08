using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace RoboRoutine
{
    public sealed class CommandPalettePresenter : IDisposable
    {
        private readonly CommandPaletteView _commandPaletteView;
        private readonly ICommandItemFactory _itemFactory;
        private readonly ICommandCrossPanelDrag _crossPanelDrag;

        private readonly List<CommandItemPresenter> _items = new();

        public CommandPalettePresenter(CommandPaletteView commandPaletteView, ICommandItemFactory itemFactory, ICommandCrossPanelDrag crossPanelDrag)
        {
            _commandPaletteView = commandPaletteView;
            _itemFactory = itemFactory;
            _crossPanelDrag = crossPanelDrag;

            _commandPaletteView.BeginDragEvent += OnBeginDrag;
            _commandPaletteView.DragEvent += OnDrag;
            _commandPaletteView.EndDragEvent += OnEndDrag;
        }

        public void AddItem(CommandItemModel itemModel)
        {
            CommandItemPresenter presenter = _itemFactory.CreatePaletteItem(itemModel);
            _commandPaletteView.AddItem(presenter.View);
            _items.Add(presenter);
        }

        private void OnBeginDrag()
        {
            _crossPanelDrag.BeginDrag();
        }

        private void OnDrag(PointerEventData eventData)
        {
            _crossPanelDrag.Drag(eventData);
        }

        private void OnEndDrag(int index)
        {
            _crossPanelDrag.EndDrag(_items[index].Model.CommandData);
        }

        public void Dispose()
        {
            _commandPaletteView.BeginDragEvent -= OnBeginDrag;
            _commandPaletteView.DragEvent -= OnDrag;
            _commandPaletteView.EndDragEvent -= OnEndDrag;

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Dispose();
            }

            _items.Clear();
        }
    }
}
