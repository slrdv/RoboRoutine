using System.Collections.Generic;
using RoboRoutine.Core;
using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class CommandItemFactory : ICommandItemFactory, ICommandItemViewSetupProvider
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
            CommandItemView view = _pool.Get(false);
            Setup(view, model);
            view.SetActive(true);

            return new CommandItemPresenter(model, view, this);
        }

        public CommandItemPresenter CreatePaletteItem(CommandItemModel model)
        {
            CommandItemPresenter itemPresenter = Create(model);
            itemPresenter.View.SetIndexVisible(false);
            return itemPresenter;
        }

        public void Setup(CommandItemView view, CommandItemModel model)
        {
            CommandType type = model.CommandData.CommandType;

            if (!_configRepository.TryGet(type, out CommandConfig config))
            {
                throw new KeyNotFoundException($"{nameof(CommandConfig)} for type {type} is not found");
            }

            if (!_setups.TryGetValue(type, out ICommandItemViewSetup viewSetup))
            {
                throw new KeyNotFoundException($"{nameof(ICommandItemViewSetup)} implementation for type {type} is not registered");
            }

            viewSetup.Setup(view, config, model.CommandData);
        }
    }
}
