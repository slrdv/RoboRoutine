using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandFactory
    {
        private readonly Dictionary<CommandType, ICommandFactory> _factories = new();

        public CommandFactory(IEnumerable<ICommandFactory> factories)
        {
            foreach (ICommandFactory factory in factories)
            {
                _factories[factory.CommandType] = factory;
            }
        }
        public ICommand Create(CommandData commandData)
        {
            if (!_factories.TryGetValue(commandData.Type, out ICommandFactory factory))
            {
                throw new KeyNotFoundException($"{nameof(ICommandFactory)} implementation for type {commandData.Type} is not registered");
            }

            return factory.Create(commandData);
        }
    }
}