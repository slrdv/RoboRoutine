using System;
using Cysharp.Threading.Tasks;

namespace RoboRoutine.Core
{
    public sealed class StateMachine : IStateMachine
    {
        public event Action<IState> StateChangedEvent;

        private readonly IStateRegistry _registry;

        private IState _state;

        public IState CurrentState => _state;

        public StateMachine(IStateRegistry registry)
        {
            _registry = registry;
        }

        public async UniTask ChangeState<T>() where T : class, IStateEntry
        {
            T state = _registry.Get<T>();
            state.Reset();
            await SwitchState(state, (state) => state.Enter());
        }

        public async UniTask ChangeState<T, TPayload>(TPayload payload) where T : class, IStateEntry<TPayload>
        {
            T state = _registry.Get<T>();
            state.Reset();
            await SwitchState(state, (state) => state.Enter(payload));
        }

        private async UniTask SwitchState<TState>(TState state, Func<TState, UniTask> enter) where TState : class, IState
        {
            if (_state != null)
            {
                await _state.Exit();

                while (!_state.IsComplete)
                {
                    await UniTask.Yield();
                }
            }

            _state = state;

            await enter(state);

            StateChangedEvent?.Invoke(_state);
        }
    }
}
