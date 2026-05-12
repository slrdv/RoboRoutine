using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridEntity
    {
        private readonly Vector2Int _size;

        public Vector2Int Size => _size;

        public GridEntity(Vector2Int size)
        {
            _size = size;
        }

    }
}