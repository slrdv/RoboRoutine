using System.Collections.Generic;

namespace RoboRoutine
{
    public sealed class GridEntityFactoryProvider
    {
        private readonly Dictionary<EntityType, IGridEntityFactory> _factories = new();

        public GridEntityFactoryProvider(IEnumerable<IGridEntityFactory> factories)
        {
            foreach (IGridEntityFactory factory in factories)
            {
                _factories[factory.EntityType] = factory;
            }
        }

        public IGridEntityController Create(GridEntityAuthoring authoring)
        {
            if (!_factories.TryGetValue(authoring.EntityType, out IGridEntityFactory factory))
            {
                throw new KeyNotFoundException($"{nameof(IGridEntityFactory)} implementation for type {authoring.EntityType} is not registered");
            }

            return factory.Create(authoring);
        }
    }
}