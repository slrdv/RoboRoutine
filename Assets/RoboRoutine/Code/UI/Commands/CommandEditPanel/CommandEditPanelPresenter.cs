using System;

namespace RoboRoutine
{
    public sealed class CommandEditPanelPresenter : IDisposable
    {
        private readonly CommandEditPanelView _view;
        private readonly CommandListPresenter _commandListPresenter;
        private readonly ICommandEditViewSetupProvider _viewSetupFactory;

        private CommandItemModel _currentModel;

        public CommandEditPanelPresenter(CommandEditPanelView view, CommandListPresenter commandListPresenter, ICommandEditViewSetupProvider viewSetupFactory)
        {
            _view = view;
            _commandListPresenter = commandListPresenter;
            _viewSetupFactory = viewSetupFactory;

            _commandListPresenter.ItemClickEvent += Show;
            _view.ApplyButtonEvent += OnApply;
            _view.CloseButtonEvent += OnClose;
        }

        public void Dispose()
        {
            _view.ApplyButtonEvent -= OnApply;
            _view.CloseButtonEvent -= OnClose;
            _commandListPresenter.ItemClickEvent -= Show;
        }

        private void Show(CommandItemModel itemModel)
        {
            _currentModel = itemModel;
            _viewSetupFactory.Setup(_view, itemModel.CommandData);
            _view.Show();
        }

        private void OnApply()
        {
            CommandData commandData = _viewSetupFactory.Apply(_currentModel.CommandData.Type);
            _currentModel.SetCommandData(commandData);
            _view.Close();
        }

        private void OnClose()
        {
            _view.Close();
        }
    }
}