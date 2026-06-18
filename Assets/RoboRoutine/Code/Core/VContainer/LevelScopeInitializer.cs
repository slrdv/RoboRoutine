using VContainer.Unity;

namespace RoboRoutine
{
    public sealed class LevelScopeInitializer : IInitializable
    {
        private readonly HistoryService _historyService;
        private readonly SequenceStorageService _sequenceStorageService;
        private readonly GridBuilder _gridBuilder;
        private readonly SimulationPanelPresenter _simulationPanelPresenter;

        public LevelScopeInitializer(
            HistoryService historyService,
            SequenceStorageService sequenceStorageService,
            GridBuilder gridBuilder,
            SimulationPanelPresenter simulationPanelPresenter)
        {
            _historyService = historyService;
            _sequenceStorageService = sequenceStorageService;
            _gridBuilder = gridBuilder;
            _simulationPanelPresenter = simulationPanelPresenter;
        }

        public void Initialize()
        {
            _gridBuilder.Build();

            _historyService.Initialize();
            _sequenceStorageService.Load();

            _simulationPanelPresenter.UpdateUI();
        }
    }
}