using System;

namespace RoboRoutine
{
    public sealed class CommandListMenuPresenter : IDisposable
    {
        private readonly CommandListMenuView _view;
        private readonly CommandPalettePresenter _palettePresenter;
        private readonly CommandListModel _commandListModel;

        public CommandListMenuPresenter(CommandListMenuView view, CommandPalettePresenter palettePresenter, CommandListModel commandListModel)
        {
            _view = view;
            _palettePresenter = palettePresenter;
            _commandListModel = commandListModel;
            _view.PaletteButtonPressedEvent += OnPaletteButtonPressed;
            _view.ClearButtonPressedEvent += OnClearButtonPressed;
        }

        private void OnPaletteButtonPressed()
        {
            _palettePresenter.ToggleActive();
        }

        private void OnClearButtonPressed()
        {
            _commandListModel.RemoveAll();
        }

        public void Dispose()
        {
            _view.PaletteButtonPressedEvent -= OnPaletteButtonPressed;
            _view.ClearButtonPressedEvent -= OnClearButtonPressed;
        }
    }
}
