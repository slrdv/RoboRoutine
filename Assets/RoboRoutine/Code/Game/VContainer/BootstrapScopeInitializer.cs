using RoboRoutine.Core;
using VContainer.Unity;

namespace RoboRoutine.Game
{
    public sealed class BootstrapScopeInitializer : IInitializable
    {
        private readonly IStateMachine _stateMachine;

        public BootstrapScopeInitializer(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            _stateMachine.ChangeState<BootstrapState>();
        }
    }
}
