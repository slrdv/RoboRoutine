using TMPro;
using UnityEngine;

namespace RoboRoutine
{
    public class NumericEntityView : GridEntityView
    {
        [SerializeField] TMP_Text _label;

        public void SetLabel(int value)
        {
            _label.text = value.ToString();
        }
    }
}