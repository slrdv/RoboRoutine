using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandFactoryProvider : ICommandFactoryProvider, ICommandDataFactory
    {
        private readonly Dictionary<CommandType, ICommandFactory> _factories = new();

        public CommandFactoryProvider(IEnumerable<ICommandFactory> factories)
        {
            foreach (ICommandFactory factory in factories)
            {
                _factories[factory.CommandType] = factory;
            }
        }
        public ICommand Create(CommandData commandData)
        {
            return GetFactory(commandData.CommandType).Create(commandData);
        }

        public CommandData CreateDefaultCommandData(CommandType commandType)
        {
            return GetFactory(commandType).CreateDefaultData();
        }

        private ICommandFactory GetFactory(CommandType commandType)
        {
            if (!_factories.TryGetValue(commandType, out ICommandFactory factory))
            {
                throw new KeyNotFoundException($"{nameof(ICommandFactory)} implementation for type {commandType} is not registered");
            }

            return factory;
        }
    }
}
