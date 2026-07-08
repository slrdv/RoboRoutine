using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;

        [SerializeField] private GridView _gridView;
        [SerializeField] private RobotView _robotView;

        [SerializeField] private SimulationPanelView _simulationPanelView;
        [SerializeField] private CommandListView _commandListView;
        [SerializeField] private ArrowPanelView _arrowPanelView;
        [SerializeField] private CommandItemView _commandItemPrefab;
        [SerializeField] private CommandPaletteView _commandPaletteView;
        [SerializeField] private CommandEditPanelView _commandEditPanelView;
        [SerializeField] private StatusIndicatorView _statusIndicatorView;
        [SerializeField] private Transform _pooledObjectsContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterGrid(builder);
            RegisterRobot(builder);

            RegisterCommandPipeline(builder);
            RegisterHistoryServices(builder);
            RegisterGameServices(builder);
            RegisterUI(builder);

            RegisterCommandSpecific(builder);

            builder.RegisterEntryPoint<TickService>(Lifetime.Singleton).As<ITickService>().As<ITickRegistry>();
            builder.Register(r => new PlayerPrefsStorage<SequenceData>("test1"), Lifetime.Singleton).As<IStorage<SequenceData>>();

            builder.RegisterEntryPoint<LevelScopeInitializer>();
        }

        private void RegisterGrid(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gridView);
            builder.Register<GridModel>(Lifetime.Singleton);
            builder.Register<GridBuilder>(Lifetime.Singleton);
            builder.Register<GridController>(Lifetime.Singleton).AsSelf().As<ISnapshotable>();

            builder.Register<GridEntityFactoryProvider>(Lifetime.Singleton);

            builder.Register<ObstacleEntityFactory>(Lifetime.Singleton).As<IGridEntityFactory>();
            builder.Register<OperandEntityFactory>(Lifetime.Singleton).As<IGridEntityFactory>();
            builder.Register<SlotEntityFactory>(Lifetime.Singleton).As<IGridEntityFactory>();
        }

        private void RegisterRobot(IContainerBuilder builder)
        {
            builder.RegisterInstance(_robotView);
            builder.Register<RobotController>(Lifetime.Singleton).AsSelf().As<ISnapshotable>();
        }

        private void RegisterCommandPipeline(IContainerBuilder builder)
        {
            builder.Register<CommandFactoryProvider>(Lifetime.Singleton).As<ICommandFactoryProvider>().As<ICommandDataFactory>();
            builder.Register<CommandSequence>(Lifetime.Singleton).As<ICommandSequence>().As<ICommandSequenceState>();
        }

        private void RegisterHistoryServices(IContainerBuilder builder)
        {
            builder.Register<SnapshotService>(Lifetime.Singleton).As<ISnapshotService>().As<ISnapshotableRegistry>();
            builder.Register<HistoryService>(Lifetime.Singleton).As<IHistoryService>();
        }

        private void RegisterGameServices(IContainerBuilder builder)
        {
            builder.Register<SimulationService>(Lifetime.Singleton).As<ISimulationService>().As<ISimulationState>();
            builder.Register<SequenceStorageService>(Lifetime.Singleton);

            builder.RegisterEager<SequenceBuilder>(Lifetime.Singleton);
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterInstance(_canvas);
            builder.RegisterInstance(_camera);
            builder.Register<CanvasService>(Lifetime.Singleton).As<ICanvasService>();

            builder.Register(r => new GameObjectPool<CommandItemView>(r, _commandItemPrefab, _pooledObjectsContainer), Lifetime.Singleton);
            builder.Register<CommandItemFactory>(Lifetime.Singleton).As<ICommandItemFactory>().As<ICommandItemViewSetupProvider>();

            builder.Register<CommandListModel>(Lifetime.Singleton);
            builder.RegisterComponent(_commandListView);
            builder.RegisterEager<CommandListPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_arrowPanelView);
            builder.RegisterEager<ArrowPanelPresenter>(Lifetime.Singleton);

            builder.Register<CommandCrossPanelDragService>(Lifetime.Singleton).As<ICommandCrossPanelDrag>().As<ICommandCrossPanelDragEventProvider>();
            builder.RegisterComponent(_commandPaletteView);
            builder.RegisterEager<CommandPalettePresenter>(Lifetime.Singleton);
            builder.Register<CommandPaletteBuilder>(Lifetime.Singleton);

            builder.Register<CommandEditViewSetupProvider>(Lifetime.Singleton).As<ICommandEditViewSetupProvider>();
            builder.RegisterInstance(_commandEditPanelView);
            builder.RegisterEager<CommandEditPanelPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_simulationPanelView);
            builder.Register<SimulationPanelPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_statusIndicatorView);
            builder.RegisterEager<StatusIndicatorPresenter>(Lifetime.Singleton);
        }

        private void RegisterCommandSpecific(IContainerBuilder builder)
        {
            builder.Register<MoveCommandFactory>(Lifetime.Singleton).As<ICommandFactory>();
            builder.Register<MovementSystem>(Lifetime.Singleton);
            builder.Register<MoveCommandItemViewSetup>(Lifetime.Singleton).As<ICommandItemViewSetup>();
            builder.Register<MoveCommandEditViewSetup>(Lifetime.Singleton).As<ICommandEditViewSetup>();

            builder.Register<PickCommandFactory>(Lifetime.Singleton).As<ICommandFactory>();
            builder.Register<PickSystem>(Lifetime.Singleton);
            builder.Register<PickCommandItemViewSetup>(Lifetime.Singleton).As<ICommandItemViewSetup>();
            builder.Register<PickCommandEditViewSetup>(Lifetime.Singleton).As<ICommandEditViewSetup>();

            builder.Register<PutCommandFactory>(Lifetime.Singleton).As<ICommandFactory>();
            builder.Register<PutSystem>(Lifetime.Singleton);
            builder.Register<PutCommandItemViewSetup>(Lifetime.Singleton).As<ICommandItemViewSetup>();
            builder.Register<PutCommandEditViewSetup>(Lifetime.Singleton).As<ICommandEditViewSetup>();

            builder.Register<EvalCommandFactory>(Lifetime.Singleton).As<ICommandFactory>();
            builder.Register<EvalSystem>(Lifetime.Singleton);
            builder.Register<EvalCommandItemViewSetup>(Lifetime.Singleton).As<ICommandItemViewSetup>();
            builder.Register<EvalCommandEditViewSetup>(Lifetime.Singleton).As<ICommandEditViewSetup>();

            builder.Register<ConditionCommandFactory>(Lifetime.Singleton).As<ICommandFactory>();
            builder.Register<ConditionCommandSystem>(Lifetime.Singleton);
            builder.Register<ConditionCommandItemViewSetup>(Lifetime.Singleton).As<ICommandItemViewSetup>();
            builder.Register<ConditionCommandEditViewSetup>(Lifetime.Singleton).As<ICommandEditViewSetup>();
        }
    }
}
