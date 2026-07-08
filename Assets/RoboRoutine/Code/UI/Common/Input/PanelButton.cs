using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class PanelButton : MonoBehaviour
    {
        public event Action PressedEvent;

        [SerializeField] private Button _button;

        public void SetEnabled(bool value)
        {
            _button.interactable = value;
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            PressedEvent?.Invoke();
        }
    }
}
