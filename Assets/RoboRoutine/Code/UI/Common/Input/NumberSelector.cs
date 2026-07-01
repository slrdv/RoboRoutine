using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class NumberSelector : MonoBehaviour
    {
        [SerializeField] private Button _decreaseButton;
        [SerializeField] private Button _increaseButton;
        [SerializeField] private TMP_Text _valueLabel;

        private int _value;
        private int _min = 0;
        private int _max = int.MaxValue;

        public int Value => _value;

        public void SetMinMax(int min, int max)
        {
            _min = min;
            _max = max;
            Reset();
        }

        public void SetValue(int value)
        {
            _value = Math.Clamp(value, _min, _max);
            _valueLabel.text = _value.ToString();
        }

        public void Reset()
        {
            SetValue(_min);
        }

        private void Increase()
        {
            SetValue(_value + 1);
        }

        private void Decrease()
        {
            SetValue(_value - 1);
        }

        private void Awake()
        {
            _decreaseButton.onClick.AddListener(Decrease);
            _increaseButton.onClick.AddListener(Increase);
        }

        private void OnDestroy()
        {
            _decreaseButton.onClick.RemoveListener(Decrease);
            _increaseButton.onClick.RemoveListener(Increase);
        }
    }
}