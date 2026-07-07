using TMPro;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class LabeledNumberInput : LabeledInput<TMP_InputField>
    {
        private int _value;
        private int _min = 0;
        private int _max = int.MaxValue;

        public int Value => _value;

        public void SetValue(int value)
        {
            _value = Mathf.Clamp(value, _min, _max);
            Input.SetTextWithoutNotify(_value.ToString());
        }

        public void SetMinMax(int min, int max)
        {
            _min = min;
            _max = max;
        }

        private void Awake()
        {
            Input.onValueChanged.AddListener(Validate);
        }

        private void Validate(string text)
        {
            if (!int.TryParse(text, out int value)) return;

            SetValue(value);
        }

        private void OnDestroy()
        {
            Input.onValueChanged.RemoveListener(Validate);
        }
    }
}