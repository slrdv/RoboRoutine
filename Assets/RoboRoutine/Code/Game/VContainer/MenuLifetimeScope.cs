using RoboRoutine.Core;
using RoboRoutine.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoboRoutine.Game
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private MenuView _menuView;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_menuView);
            builder.RegisterEager<MenuPresenter>(Lifetime.Singleton);

            builder.RegisterEntryPoint<MenuScopeInitializer>(Lifetime.Singleton);
        }
    }
}
