using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoboRoutine.UI
{
    public sealed class LevelMenuView : MonoBehaviour
    {
        public event Action NextButtonPressedEvent;
        public event Action MenuButtonPressedEvent;
        public event Action ExitButtonPressedEvent;

        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _exitButton;

        private void OnNextButtonPressed()
        {
            NextButtonPressedEvent?.Invoke();
        }

        private void OnMenuButtonPressed()
        {
            MenuButtonPressedEvent?.Invoke();
        }

        private void OnExitButtonPressed()
        {
            ExitButtonPressedEvent?.Invoke();
        }

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(OnNextButtonPressed);
            _menuButton.onClick.AddListener(OnMenuButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextButtonPressed);
            _menuButton.onClick.RemoveListener(OnMenuButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
        }
    }
}
