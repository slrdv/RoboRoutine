using System;

namespace RoboRoutine
{
    public sealed class MenuPresenter : IDisposable
    {
        private readonly MenuView _view;
        private readonly ILevelManager _levelManager;

        public MenuPresenter(MenuView view, ILevelManager levelManager)
        {
            _view = view;
            _levelManager = levelManager;
            _view.StartButtonPressedEvent += OnStartButtonPressed;
            _view.ExitButtonPressedEvent += OnExitButtonPressed;
        }

        private void OnStartButtonPressed()
        {
            _levelManager.LoadNext();
        }

        private void OnExitButtonPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Dispose()
        {
            _view.StartButtonPressedEvent -= OnStartButtonPressed;
            _view.ExitButtonPressedEvent -= OnExitButtonPressed;
        }
    }
}
