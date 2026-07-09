using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public interface IState
    {
        bool IsComplete { get; }
        UniTask Exit();
        void Reset();
    }

    public interface IStateEntry : IState
    {
        UniTask Enter();
    }

    public interface IStateEntry<TPayload> : IState
    {
        UniTask Enter(TPayload payload);
    }

    public abstract class StateEntryCompletion
    {
        private bool _complete;

        public bool IsComplete => _complete;

        public void Reset()
        {
            _complete = false;
        }

        protected void Complete()
        {
            _complete = true;
        }
    }

    public abstract class StateEntryBase : StateEntryCompletion, IStateEntry
    {
        public abstract UniTask Exit();
        public abstract UniTask Enter();
    }

    public abstract class StateEntryBase<TPayload> : StateEntryCompletion, IStateEntry<TPayload>
    {
        public abstract UniTask Exit();
        public abstract UniTask Enter(TPayload payload);
    }
}
