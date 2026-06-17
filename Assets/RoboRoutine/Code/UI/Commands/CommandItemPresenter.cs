using System;
using R3;

namespace RoboRoutine
{
    public sealed class CommandItemPresenter: IDisposable
    {
        private readonly CommandItemModel _model;
        private readonly CommandItemView _view;

        private readonly CompositeDisposable _subscriptions = new();

        public CommandItemView View => _view;

        public CommandItemPresenter(CommandItemModel model, CommandItemView view)
        {
            _model = model;
            _view = view;

            _model.Index.Subscribe(_view.SetIndex).AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
            if (_view != null)
            {
                _view.Release();
            }
        }
    }
}