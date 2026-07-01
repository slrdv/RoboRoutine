using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandEditViewSetupFactory : ICommandEditViewSetupFactory
    {
        private readonly Dictionary<CommandType, ICommandEditViewSetup> _setups = new();

        public CommandEditViewSetupFactory(IEnumerable<ICommandEditViewSetup> setups)
        {
            foreach (ICommandEditViewSetup setup in setups)
            {
                _setups[setup.CommandType] = setup;
            }
        }

        public void Setup(CommandEditPanelView view, CommandData data)
        {
            GetSetup(data.Type).Setup(view, data);
        }

        public CommandData Apply(CommandType type)
        {
            return GetSetup(type).Apply();
        }

        private ICommandEditViewSetup GetSetup(CommandType type)
        {
            if (!_setups.TryGetValue(type, out ICommandEditViewSetup setup))
            {
                throw new KeyNotFoundException($"{nameof(ICommandEditViewSetup)} for type {type} is not registered");
            }

            return setup;
        }
    }
}