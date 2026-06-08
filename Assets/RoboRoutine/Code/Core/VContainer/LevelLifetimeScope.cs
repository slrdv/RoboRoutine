using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private GridView _gridView;
        [SerializeField] private RobotView _robotView;

        [SerializeField] private CommandListView _commandListView;
        [SerializeField] private CommandItemView _commandItemPrefab;
        [SerializeField] private Transform _pooledObjectsContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterGrid(builder);
            RegisterRobot(builder);

            RegisterCommandPipeline(builder);
            RegisterHistoryServices(builder);

            RegisterUI(builder);

            builder.RegisterEntryPoint<TickService>(Lifetime.Singleton).As<ITickService>().As<ITickRegistry>();
            builder.RegisterEntryPoint<LevelScopeInitializer>();
        }

        private void RegisterGrid(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gridView);
            builder.Register<GridModel>(Lifetime.Singleton);
            builder.Register<GridBuilder>(Lifetime.Singleton);
            builder.Register<GridController>(Lifetime.Singleton);
        }

        private void RegisterRobot(IContainerBuilder builder)
        {
            builder.RegisterInstance(_robotView);
            builder.Register<RobotController>(Lifetime.Singleton);
        }

        private void RegisterCommandPipeline(IContainerBuilder builder)
        {
            builder.Register<CommandFactory>(Lifetime.Singleton);
            builder.Register<MovementSystem>(Lifetime.Singleton);
            builder.Register<CommandSequence>(Lifetime.Singleton);
        }

        private void RegisterHistoryServices(IContainerBuilder builder)
        {
            builder.Register<SnapshotService>(Lifetime.Singleton).AsSelf().As<ISnapshotableRegistry>();
            builder.Register<HistoryService>(Lifetime.Singleton);
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.Register(r => new GameObjectPool<CommandItemView>(r, _commandItemPrefab, _pooledObjectsContainer), Lifetime.Singleton);

            builder.Register<CommandItemFactory>(Lifetime.Singleton);

            builder.Register<CommandListModel>(Lifetime.Singleton);
            builder.RegisterInstance(_commandListView);
            builder.RegisterEntryPoint<CommandListPresenter>();
        }
    }
}
