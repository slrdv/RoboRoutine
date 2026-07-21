using TMPro;
using UnityEngine;

namespace RoboRoutine.Features
{
    public class NumericEntityView : GridEntityView, INumericEntityView
    {
        [SerializeField] TMP_Text _label;

        public void SetLabel(int value)
        {
            _label.text = value.ToString();
        }
    }
}
