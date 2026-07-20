using System;

namespace RoboRoutine.UI
{
    public sealed class LevelCompletePresenter : ILevelCompleteListener, IDisposable
    {
        private readonly LevelCompletePanelView _view;
        private readonly ILevelManager _levelManager;

        private LevelResult _result;

        public LevelCompletePresenter(LevelCompletePanelView view, ILevelManager levelManager)
        {
            _view = view;
            _levelManager = levelManager;

            _view.NextButtonPressedEvent += OnNextButtonPressed;
            _view.CloseButtonPressedEvent += OnCloseButtonPressed;
        }

        public void OnLevelComplete(LevelResult result)
        {
            _result = result;
            _view.Show();
        }

        private void OnNextButtonPressed()
        {
            _levelManager.OnLevelComplete(_result);
        }

        private void OnCloseButtonPressed()
        {
            _view.Hide();
        }

        public void Dispose()
        {
            _view.NextButtonPressedEvent -= OnNextButtonPressed;
            _view.CloseButtonPressedEvent -= OnCloseButtonPressed;
        }
    }
}
