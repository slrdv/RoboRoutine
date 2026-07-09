using VContainer.Unity;

namespace RoboRoutine
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
