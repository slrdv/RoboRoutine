using VContainer.Unity;

namespace RoboRoutine
{
    public sealed class LevelBuilder : IStartable
    {
        private readonly IHistoryService _historyService;
        private readonly SequenceStorageService _sequenceStorageService;
        private readonly GridBuilder _gridBuilder;
        private readonly SimulationPanelPresenter _simulationPanelPresenter;
        private readonly CommandPaletteBuilder _commandPaletteBuilder;
        private readonly ArrowPanelPresenter _arrowPanelPresenter;
        private readonly ILevelManager _levelManager;

        public LevelBuilder(
            IHistoryService historyService,
            SequenceStorageService sequenceStorageService,
            GridBuilder gridBuilder,
            SimulationPanelPresenter simulationPanelPresenter,
            CommandPaletteBuilder commandPaletteBuilder,
            ArrowPanelPresenter arrowPanelPresenter,
            ILevelManager levelManager)
        {
            _historyService = historyService;
            _sequenceStorageService = sequenceStorageService;
            _gridBuilder = gridBuilder;
            _simulationPanelPresenter = simulationPanelPresenter;
            _commandPaletteBuilder = commandPaletteBuilder;
            _arrowPanelPresenter = arrowPanelPresenter;
            _levelManager = levelManager;
        }

        public void Start()
        {
            _gridBuilder.Build();

            _historyService.Initialize();
            _sequenceStorageService.Load();

            _commandPaletteBuilder.Build();
            _simulationPanelPresenter.UpdateUI();

            _arrowPanelPresenter.Initialize();

            _levelManager.OnLoadLevelComplete();
        }
    }
}
