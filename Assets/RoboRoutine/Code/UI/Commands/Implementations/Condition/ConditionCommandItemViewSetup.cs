using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class ConditionCommandItemViewSetup : ICommandItemViewSetup
    {
        public CommandType CommandType => CommandType.Condition;

        public void Setup(CommandItemView view, CommandConfig config, CommandData commandData)
        {
            ConditionCommandData data = (ConditionCommandData)commandData;
            ConditionCommandConfig itemConfig = (ConditionCommandConfig)config;

            view.SetIcon(itemConfig.Icon);
            view.SetLabel($"{itemConfig.GetLabel(data.ConditionType)} {data.Value}");
        }
    }
}
