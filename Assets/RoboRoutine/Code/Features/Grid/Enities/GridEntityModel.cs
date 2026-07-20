using UnityEngine;

namespace RoboRoutine.Features
{
    public class GridEntityModel
    {
        private readonly EntityType _entityType;
        private readonly Vector2Int _size;

        public Vector2Int Size => _size;
        public EntityType EntityType => _entityType;

        public GridEntityModel(Vector2Int size, EntityType entityType)
        {
            _size = size;
            _entityType = entityType;
        }
    }
}
