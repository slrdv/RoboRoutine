using System;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class CommandListMenuView : MonoBehaviour
    {
        public event Action PaletteButtonPressedEvent;
        public event Action ClearButtonPressedEvent;

        [SerializeField] private PanelButton _paletteButton;
        [SerializeField] private PanelButton _clearButton;

        private void OnPaletteButtonPressed()
        {
            PaletteButtonPressedEvent?.Invoke();
        }

        private void OnClearButtonPressed()
        {
            ClearButtonPressedEvent?.Invoke();
        }

        private void Awake()
        {
            _paletteButton.PressedEvent += OnPaletteButtonPressed;
            _clearButton.PressedEvent += OnClearButtonPressed;
        }

        private void OnDestroy()
        {
            _paletteButton.PressedEvent -= OnPaletteButtonPressed;
            _clearButton.PressedEvent -= OnClearButtonPressed;
        }
    }
}
