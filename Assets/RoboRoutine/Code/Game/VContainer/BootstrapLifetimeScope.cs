using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class BootstraLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BootstrapScopeInitializer>();
        }
    }
}
