using UnityEngine;

namespace RoboRoutine
{
    public sealed class RobotView : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;

        public Vector2 GetPositionXZ()
        {
            return transform.position.ToXZ();
        }
        public void SetPositionXZ(Vector2 position)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.y);
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
    }
}