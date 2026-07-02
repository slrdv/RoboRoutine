namespace RoboRoutine
{
    public sealed class MoveCommandEditViewSetup : ICommandEditViewSetup
    {
        public CommandType CommandType => CommandType.Move;

        private LabeledDropdown _dropdown;
        private LabeledNumberSelector _numberSelector;

        public CommandData Apply()
        {
            return new MoveCommandData(EnumUtils.GetMember<MoveDirection>(_dropdown.GetCurrentName()), _numberSelector.Input.Value);
        }

        public void Setup(CommandEditPanelView view, CommandData data)
        {
            MoveCommandData moveData = (MoveCommandData)data;

            _dropdown = view.AddDropdown("Direction", EnumUtils.GetNames<MoveDirection>());
            _dropdown.SetSelected(moveData.MoveDirection.ToString());

            _numberSelector = view.AddNumberSelector("Distance", 1, 10);
            _numberSelector.Input.SetValue(moveData.Distance);
        }
    }
}