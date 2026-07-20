using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine.UI
{
    public sealed class CommandEditPanelView : MonoBehaviour
    {
        public event Action ApplyButtonEvent;
        public event Action CloseButtonEvent;

        [SerializeField] private PanelButton _applyButton;
        [SerializeField] private PanelButton _closeButton;

        [SerializeField] private Transform _inputContainer;
        [SerializeField] private LabeledDropdown _dropdownPrefab;
        [SerializeField] private LabeledNumberSelector _numberSelectorPrefab;
        [SerializeField] private LabeledNumberInput _numberInputPrefab;

        private readonly List<LabeledInput> _inputs = new();

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            ResetInputs();
        }

        public void ResetInputs()
        {
            for (int i = 0; i < _inputs.Count; i++)
            {
                Destroy(_inputs[i].gameObject);
            }

            _inputs.Clear();
        }

        public LabeledNumberSelector AddNumberSelector(string label, int min, int max)
        {
            LabeledNumberSelector input = CreateInput(_numberSelectorPrefab, label);
            input.Input.SetMinMax(min, max);
            return input;
        }

        public LabeledNumberInput AddNumberInput(string label, int min, int max)
        {
            LabeledNumberInput input = CreateInput(_numberInputPrefab, label);
            input.SetMinMax(min, max);
            return input;
        }

        public LabeledDropdown AddDropdown(string label, List<string> options)
        {
            LabeledDropdown input = CreateInput(_dropdownPrefab, label);
            input.Input.ClearOptions();
            input.Input.AddOptions(options);
            return input;
        }

        private void Awake()
        {
            _applyButton.PressedEvent += OnApplyButtonPressed;
            _closeButton.PressedEvent += OnCloseButtonPressed;
            Close();
        }

        private void OnDestroy()
        {
            _applyButton.PressedEvent -= OnApplyButtonPressed;
            _closeButton.PressedEvent -= OnCloseButtonPressed;
        }

        private void OnApplyButtonPressed()
        {
            ApplyButtonEvent?.Invoke();
        }

        private void OnCloseButtonPressed()
        {
            CloseButtonEvent?.Invoke();
        }

        private TInput CreateInput<TInput>(TInput prefab, string label) where TInput : LabeledInput
        {
            TInput input = Instantiate(prefab, _inputContainer);
            input.SetText(label);
            _inputs.Add(input);

            return input;
        }
    }
}
