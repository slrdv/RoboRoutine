using System.Collections.Generic;
using VContainer;

namespace RoboRoutine.Core
{
    public sealed class StateRegistry : IStateRegistry
    {
        private readonly IObjectResolver _resolver;

        public StateRegistry(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T Get<T>() where T : class, IState
        {
            if (!_resolver.TryResolve(out T state)) throw new KeyNotFoundException($"State {typeof(T)} is not registered");
            return state;
        }
    }
}
