using UnityEngine;

namespace RoboRoutine
{
    public sealed class RobotView : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private Transform _itemRoot;
        [SerializeField] private Vector3 _itemScale = new Vector3(0.5f, 0.5f, 0.5f);

        public Vector2 GetPositionXZ()
        {
            return transform.position.ToXZ();
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public void SetPositionXZ(Vector2 position)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.y);
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void MoveTowardsXZ(Vector2 targetPosition, float delta)
        {
            Vector2 newPosition = Vector2.MoveTowards(GetPositionXZ(), targetPosition, delta * _speed);
            SetPositionXZ(newPosition);
        }

        public void LookAtXZ(Vector2 targetPosition)
        {
            transform.LookAt(targetPosition.WithY());
        }

        public void AttachItem(GridEntityView item)
        {
            item.SetParent(_itemRoot);
            item.SetPosition(_itemRoot.transform.position);
            item.SetScale(_itemScale);
        }
    }
}