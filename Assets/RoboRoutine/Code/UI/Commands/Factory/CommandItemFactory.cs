using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class CommandItemFactory
    {
        private readonly GameObjectPool<CommandItemView> _pool;
        private readonly IRepository<CommandType, CommandConfig> _configRepository;
        private readonly Dictionary<CommandType, ICommandItemViewSetup> _setups = new();

        public CommandItemFactory(GameObjectPool<CommandItemView> pool, IRepository<CommandType, CommandConfig> configRepository, IEnumerable<ICommandItemViewSetup> setups)
        {
            _pool = pool;
            _configRepository = configRepository;

            foreach (ICommandItemViewSetup setup in setups)
            {
                _setups[setup.CommandType] = setup;
            }
        }

        public CommandItemPresenter Create(CommandItemModel model)
        {
            CommandType type = model.CommandData.Type;

            if (!_configRepository.TryGet(type, out CommandConfig config))
            {
                throw new KeyNotFoundException($"{nameof(CommandConfig)} for type {type} is not found");
            }

            if (!_setups.TryGetValue(type, out ICommandItemViewSetup viewSetup))
            {
                throw new KeyNotFoundException($"{nameof(ICommandItemViewSetup)} implementation for type {type} is not registered");
            }

            CommandItemView view = _pool.Get(false);
            viewSetup.Setup(view, config, model.CommandData);
            view.SetActive(true);

            return new CommandItemPresenter(model, view);
        }
    }
}