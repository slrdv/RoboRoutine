using System;
using VContainer.Unity;

namespace RoboRoutine
{
    public sealed class LevelMenuPresenter : IDisposable, IInitializable
    {
        private readonly LevelMenuView _view;
        private readonly ILevelManager _levelManager;
        private readonly IGameManager _gameManager;
        private readonly IInputHandler _inputHandler;

        private bool _isOpen;

        public LevelMenuPresenter(LevelMenuView levelMenuView, ILevelManager levelManager, IGameManager gameManager, IInputHandler inputHandler)
        {
            _view = levelMenuView;
            _levelManager = levelManager;
            _gameManager = gameManager;
            _inputHandler = inputHandler;

            _view.NextButtonPressedEvent += OnNextButtonPressed;
            _view.MenuButtonPressedEvent += OnMenuButtonPressed;
            _view.ExitButtonPressedEvent += OnExitButtonPressed;

            inputHandler.IgmActionEvent += ToggleShow;
        }

        public void Initialize()
        {
            SetOpen(false);
        }

        public void Dispose()
        {
            _inputHandler.IgmActionEvent -= ToggleShow;

            _view.NextButtonPressedEvent -= OnNextButtonPressed;
            _view.MenuButtonPressedEvent -= OnMenuButtonPressed;
            _view.ExitButtonPressedEvent -= OnExitButtonPressed;
        }

        private void OnNextButtonPressed()
        {
            _levelManager.LoadNext();
        }

        private void OnMenuButtonPressed()
        {
            _levelManager.LoadMenu();
        }

        private void OnExitButtonPressed()
        {
            _gameManager.Exit();
        }

        private void ToggleShow()
        {
            SetOpen(!_isOpen);
        }

        private void SetOpen(bool value)
        {
            _isOpen = value;
            _view.gameObject.SetActive(_isOpen);
        }
    }
}
