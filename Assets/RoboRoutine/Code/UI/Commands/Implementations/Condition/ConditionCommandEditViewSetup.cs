using RoboRoutine.Core;
using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class ConditionCommandEditViewSetup : ICommandEditViewSetup
    {
        public CommandType CommandType => CommandType.Condition;

        private int _targetIndex;
        private LabeledDropdown _dropdown;
        private LabeledNumberInput _numberInput;

        public CommandData Apply()
        {
            return new ConditionCommandData(EnumUtils.GetMember<ConditionType>(_dropdown.GetCurrentName()), _numberInput.Value, _targetIndex);
        }

        public void Setup(CommandEditPanelView view, CommandData data)
        {
            ConditionCommandData conditionData = (ConditionCommandData)data;

            _targetIndex = conditionData.TargetIndex;

            _dropdown = view.AddDropdown("Condition", EnumUtils.GetNames<ConditionType>());
            _dropdown.SetSelected(conditionData.ConditionType.ToString());

            _numberInput = view.AddNumberInput("Value", int.MinValue, int.MaxValue);
            _numberInput.SetValue(conditionData.Value);
        }
    }
}
