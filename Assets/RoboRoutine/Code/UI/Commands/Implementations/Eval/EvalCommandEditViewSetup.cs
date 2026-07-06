namespace RoboRoutine
{
    public sealed class EvalCommandEditViewSetup : ICommandEditViewSetup
    {
        public CommandType CommandType => CommandType.Eval;

        private LabeledDropdown _dropdown;

        public CommandData Apply()
        {
            return new EvalCommandData(EnumUtils.GetMember<EvalType>(_dropdown.GetCurrentName()));
        }

        public void Setup(CommandEditPanelView view, CommandData data)
        {
            EvalCommandData evalData = (EvalCommandData)data;

            _dropdown = view.AddDropdown("Operation", EnumUtils.GetNames<EvalType>());
            _dropdown.SetSelected(evalData.EvalType.ToString());
        }
    }
}