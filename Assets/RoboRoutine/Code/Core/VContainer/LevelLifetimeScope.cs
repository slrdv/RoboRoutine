using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private GridView _gridView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gridView);
            builder.Register<GridModel>(Lifetime.Singleton).AsSelf();
            builder.Register<GridBuilder>(Lifetime.Singleton);

            builder.RegisterEntryPoint<LevelScopeInitializer>();
        }
    }
}
