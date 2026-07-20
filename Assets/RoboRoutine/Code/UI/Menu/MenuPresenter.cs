using System;

namespace RoboRoutine.UI
{
    public sealed class MenuPresenter : IDisposable
    {
        private readonly MenuView _view;
        private readonly ILevelManager _levelManager;
        private readonly IGameManager _gameManager;

        public MenuPresenter(MenuView view, ILevelManager levelManager, IGameManager gameManager)
        {
            _view = view;
            _levelManager = levelManager;
            _gameManager = gameManager;
            _view.StartButtonPressedEvent += OnStartButtonPressed;
            _view.ExitButtonPressedEvent += OnExitButtonPressed;
        }

        private void OnStartButtonPressed()
        {
            _levelManager.LoadNext();
        }

        private void OnExitButtonPressed()
        {
            _gameManager.Exit();
        }

        public void Dispose()
        {
            _view.StartButtonPressedEvent -= OnStartButtonPressed;
            _view.ExitButtonPressedEvent -= OnExitButtonPressed;
        }
    }
}
