using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridModel
    {
        private RectInt _bounds = RectInt.zero;
        private readonly Dictionary<Vector2Int, GridEntity> _entities = new();

        public RectInt Bounds => _bounds;

        public void AddEntity(GridEntity entity, Vector2Int position)
        {
            _entities.Add(position, entity);
            
            _bounds.xMin = Mathf.Min(_bounds.xMin, position.x);
            _bounds.yMin = Mathf.Min(_bounds.yMin, position.y);
            _bounds.xMax = Mathf.Max(_bounds.xMax, position.x + entity.Size.x);
            _bounds.yMax = Mathf.Max(_bounds.yMax, position.y + entity.Size.y);
        }
    }
}