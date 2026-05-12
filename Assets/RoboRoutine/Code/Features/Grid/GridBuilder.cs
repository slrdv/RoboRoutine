using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridBuilder
    {
        private readonly GridModel _gridModel;
        private readonly GridView _gridView;

        public GridBuilder(GridModel gridModel, GridView gridView)
        {
            _gridModel = gridModel;
            _gridView = gridView;
        }

        public void Build()
        {
            GridEntityAuthoring[] authorings = Object.FindObjectsByType<GridEntityAuthoring>(FindObjectsSortMode.None);
            for (int i = 0; i < authorings.Length; i++)
            {
                GridEntityAuthoring authoring = authorings[i];
                GridEntity entity = new GridEntity(authoring.GetActualSize());

                _gridModel.AddEntity(entity, authoring.GetOrigin());
            }

            _gridView.Build(_gridModel.Bounds);
        }
    }
}