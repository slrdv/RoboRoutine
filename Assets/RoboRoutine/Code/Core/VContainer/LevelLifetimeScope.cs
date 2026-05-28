using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private GridView _gridView;
        [SerializeField] private RobotView _robotView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterGrid(builder);
            RegisterRobot(builder);

            RegisterCommandPipeline(builder);

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
    }
}
