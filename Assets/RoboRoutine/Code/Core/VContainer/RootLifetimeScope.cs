using VContainer;
using VContainer.Unity;

namespace RoboRoutine
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register(r => new ScriptableRepository<CommandType, CommandConfig>(PathConstants.CommandConfigPath), Lifetime.Singleton).As<IRepository<CommandType, CommandConfig>>();
        }
    }
}
