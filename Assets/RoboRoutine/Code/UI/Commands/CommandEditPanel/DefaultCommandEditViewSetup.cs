using RoboRoutine.Features;
using VContainer;

namespace RoboRoutine.UI
{
    public abstract class DefaultEditViewSetup : ICommandEditViewSetup
    {
        [Inject] private readonly ICommandDataFactory _dataFactory;

        public abstract CommandType CommandType { get; }

        public CommandData Apply()
        {
            return _dataFactory.CreateDefaultCommandData(CommandType);
        }
        public void Setup(CommandEditPanelView view, CommandData data) { }
    }
}
