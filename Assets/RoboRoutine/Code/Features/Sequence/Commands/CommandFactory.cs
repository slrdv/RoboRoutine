using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandFactory : ICommandDataFactory
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
            return GetFactory(commandData.Type).Create(commandData);
        }

        public CommandData CreateDefaultCommandData(CommandType commandType)
        {
            return GetFactory(commandType).CreateDefaultData();
        }

        public CommandData CloneData(CommandData commandData)
        {
            return GetFactory(commandData.Type).CloneData(commandData);
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