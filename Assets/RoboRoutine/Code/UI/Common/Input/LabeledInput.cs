using TMPro;
using UnityEngine;

namespace RoboRoutine.UI
{
    public abstract class LabeledInput : MonoBehaviour
    {
        [SerializeField] TMP_Text _label;

        public void SetText(string text)
        {
            _label.text = text;
        }
    }

    public abstract class LabeledInput<TInput> : LabeledInput where TInput : MonoBehaviour
    {
        [SerializeField] TInput _input;

        public TInput Input => _input;
    }
}
