using System;
using R3;

namespace RoboRoutine
{
    public sealed class CommandItemPresenter : IDisposable
    {
        public event Action<CommandItemPresenter> ClickEvent;

        private readonly CommandItemModel _model;
        private readonly CommandItemView _view;
        private readonly ICommandItemViewSetupProvider _setupFactory;

        private readonly CompositeDisposable _subscriptions = new();

        public CommandItemView View => _view;
        public CommandItemModel Model => _model;

        public CommandItemPresenter(CommandItemModel model, CommandItemView view, ICommandItemViewSetupProvider setupFactory)
        {
            _model = model;
            _view = view;
            _setupFactory = setupFactory;

            _model.Index.Subscribe(_view.SetIndex).AddTo(_subscriptions);
            _model.DataChangedEvent += OnDataChanged;
            _view.ClickEvent += OnClick;
        }

        public void Dispose()
        {
            _view.ClickEvent -= OnClick;
            _model.DataChangedEvent -= OnDataChanged;
            _subscriptions.Dispose();
            if (_view != null)
            {
                _view.Release();
            }
        }

        private void OnDataChanged(CommandItemModel _)
        {
            _setupFactory.Setup(_view, _model);
        }

        private void OnClick()
        {
            ClickEvent?.Invoke(this);
        }
    }
}
