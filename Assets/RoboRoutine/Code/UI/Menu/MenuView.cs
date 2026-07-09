using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class MenuView : MonoBehaviour
    {
        public event Action StartButtonPressedEvent;
        public event Action ExitButtonPressedEvent;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private void OnStartButtonPressed()
        {
            StartButtonPressedEvent?.Invoke();
        }

        private void OnExitButtonPressed()
        {
            ExitButtonPressedEvent?.Invoke();
        }

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
        }
    }
}
