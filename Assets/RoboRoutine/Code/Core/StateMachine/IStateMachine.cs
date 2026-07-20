using System;
using Cysharp.Threading.Tasks;

namespace RoboRoutine.Core
{
    public interface IStateMachine
    {
        public event Action<IState> StateChangedEvent;

        public IState CurrentState { get; }

        UniTask ChangeState<T>() where T : class, IStateEntry;
        UniTask ChangeState<T, TPayload>(TPayload payload) where T : class, IStateEntry<TPayload>;
    }
}
