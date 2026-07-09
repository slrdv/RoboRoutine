using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private LoadingScreen _loadingScreenPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register(r => new ScriptableRepository<CommandType, CommandConfig>(Constants.CommandConfigPath), Lifetime.Singleton).As<IRepository<CommandType, CommandConfig>>();
            builder.Register(r => new ScriptableRepository<int, LevelConfig>(Constants.LevelConfigPath), Lifetime.Singleton).As<IRepository<int, LevelConfig>>();

            builder.Register<PlayerPrefsStorage<SequenceData>>(Lifetime.Singleton).As<IStorage<string, SequenceData>>();

            builder.RegisterComponentInNewPrefab(_loadingScreenPrefab, Lifetime.Singleton).As<ILoadingScreen>();

            builder.Register<StateRegistry>(Lifetime.Singleton).As<IStateRegistry>();
            builder.Register<StateMachine>(Lifetime.Singleton).As<IStateMachine>();

            builder.Register<LevelRegistry>(Lifetime.Singleton);
            builder.RegisterEager<LevelManager>(Lifetime.Singleton).AsSelf().As<ILevelManager>();

            builder.Register<GameManager>(Lifetime.Singleton).As<IGameManager>();

            RegisterStates(builder);
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadLevelState>(Lifetime.Singleton);
            builder.Register<RunLevelState>(Lifetime.Singleton);
        }
    }
}
