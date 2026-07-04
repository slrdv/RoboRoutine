using UnityEngine;

namespace RoboRoutine
{
    [DisallowMultipleComponent]
    public class GridEntityAuthoring : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size = Vector2Int.one;
        [SerializeField] private FootpintRotation _rotation = FootpintRotation.R0;
        [SerializeField] private EntityType _entityType = EntityType.Obstacle;

        public Vector2Int Size => _size;
        public FootpintRotation Rotation => _rotation;
        public EntityType EntityType => _entityType;

        public Vector2Int GetActualSize()
        {
            return ((int)_rotation % 2 == 1) ? new Vector2Int(_size.y, _size.x) : _size;
        }

        public Vector2Int GetOrigin()
        {
            Vector2Int size = GetActualSize();
            Vector3 pos = transform.position;

            return new Vector2Int(Mathf.FloorToInt(pos.x - (size.x * 0.5f)), Mathf.FloorToInt(pos.z - (size.y * 0.5f)));
        }
    }
}