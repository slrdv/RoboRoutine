namespace RoboRoutine
{
    public sealed class EvalCommandItemViewSetup : ICommandItemViewSetup
    {
        public CommandType CommandType => CommandType.Eval;

        public void Setup(CommandItemView view, CommandConfig config, CommandData commandData)
        {
            EvalCommandData evalData = (EvalCommandData)commandData;

            EvalCommandConfig evalConfig = (EvalCommandConfig)config;
            EvalCommandConfig.ItemConfig itemConfig = evalConfig.GetItemConfig(evalData.EvalType);

            view.SetIcon(itemConfig.Icon);
            view.SetLabel(itemConfig.DisplayName);
        }
    }
}
