using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridController
    {
        private readonly GridModel _gridModel;
        private readonly GridView _gridView;

        public GridController(GridModel gridModel, GridView gridView)
        {
            _gridModel = gridModel;
            _gridView = gridView;
        }

        public Vector2 GetCellCenterWorldPositionXZ(Vector2Int cell)
        {
            return new Vector2(cell.x + 0.5f, cell.y + 0.5f);
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            return WorldToCell(worldPosition.ToXZ());
        }

        public Vector2Int WorldToCell(Vector2 worldPositionXZ)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPositionXZ.x), Mathf.FloorToInt(worldPositionXZ.y));
        }
    }
}