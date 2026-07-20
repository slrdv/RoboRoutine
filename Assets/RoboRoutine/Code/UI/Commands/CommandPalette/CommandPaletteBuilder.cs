using System.Collections.Generic;
using RoboRoutine.Core;
using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class CommandPaletteBuilder
    {
        private readonly CommandPalettePresenter _palettePresenter;
        private readonly ICommandDataFactory _dataFactory;
        private readonly IRepository<CommandType, CommandConfig> _repository;

        public CommandPaletteBuilder(CommandPalettePresenter palettePresenter, IRepository<CommandType, CommandConfig> repository, ICommandDataFactory dataFactory)
        {
            _palettePresenter = palettePresenter;
            _repository = repository;
            _dataFactory = dataFactory;
        }

        public void Build()
        {
            IReadOnlyCollection<CommandConfig> configs = _repository.GetAll();
            foreach (CommandConfig config in configs)
            {
                CommandData data = _dataFactory.CreateDefaultCommandData(config.Type);
                _palettePresenter.AddItem(new CommandItemModel(data));
            }
        }
    }
}
