using System;
using UnityEngine;

namespace RoboRoutine.UI
{
    public sealed class LevelCompletePanelView : MonoBehaviour
    {
        public event Action NextButtonPressedEvent;
        public event Action CloseButtonPressedEvent;

        [SerializeField] private PanelButton _nextButton;
        [SerializeField] private PanelButton _closeButton;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnNextButtonPressed()
        {
            NextButtonPressedEvent?.Invoke();
        }

        private void OnCloseButtonPressed()
        {
            CloseButtonPressedEvent?.Invoke();
        }

        private void Awake()
        {
            _nextButton.PressedEvent += OnNextButtonPressed;
            _closeButton.PressedEvent += OnCloseButtonPressed;
        }

        private void OnDestroy()
        {
            _nextButton.PressedEvent -= OnNextButtonPressed;
            _closeButton.PressedEvent -= OnCloseButtonPressed;
        }
    }
}
