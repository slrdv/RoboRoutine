using System.Collections.Generic;
using TMPro;

namespace RoboRoutine.UI
{
    public sealed class LabeledDropdown : LabeledInput<TMP_Dropdown>
    {
        public void SetSelected(string name)
        {
            int index = -1;
            for (int i = 0; i < Input.options.Count; i++)
            {
                if (Input.options[i].text.Equals(name))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1) throw new KeyNotFoundException($"Option '{name}' not found.");

            Input.SetValueWithoutNotify(index);
            Input.RefreshShownValue();
        }

        public string GetCurrentName()
        {
            return Input.options[Input.value].text;
        }
    }
}
