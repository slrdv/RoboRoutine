using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridBuilder
    {
        private readonly GridController _grid;
        private readonly GridEntityFactoryProvider _factory;

        public GridBuilder(GridController grid, GridEntityFactoryProvider factory)
        {
            _grid = grid;
            _factory = factory;
        }

        public void Build()
        {
            GridEntityAuthoring[] authorings = Object.FindObjectsByType<GridEntityAuthoring>(FindObjectsSortMode.None);
            for (int i = 0; i < authorings.Length; i++)
            {
                GridEntityAuthoring authoring = authorings[i];
                if (authoring.EntityType == EntityType.None) continue;

                IGridEntityController gridEntityController = _factory.Create(authoring);
                _grid.AddEntity(gridEntityController, authoring.GetOrigin());
            }

            _grid.Build();
        }
    }
}
