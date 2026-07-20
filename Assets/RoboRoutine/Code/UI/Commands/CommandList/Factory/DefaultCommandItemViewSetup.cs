using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public abstract class DefaultCommandItemViewSetup : ICommandItemViewSetup
    {
        public abstract CommandType CommandType { get; }

        public void Setup(CommandItemView view, CommandConfig config, CommandData commandData)
        {
            view.SetIcon(config.Icon);
            view.SetLabel(config.DisplayName);
            view.SetClickEnable(false);
        }
    }
}
