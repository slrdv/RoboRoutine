using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class TestRobotView : IRobotView
    {
        private Vector2 _position;

        public Transform ItemRoot => null;
        public Vector3 ItemScale => Vector3.one;

        public Vector2 GetPositionXZ()
        {
            return _position;
        }

        public Quaternion GetRotation()
        {
            return Quaternion.identity;
        }

        public void SetPositionXZ(Vector2 position)
        {
            _position = position;
        }

        public void SetRotation(Quaternion rotation) { }

        public void MoveTowardsXZ(Vector2 targetPosition, float delta)
        {
            SetPositionXZ(targetPosition);
        }

        public void LookAtXZ(Vector2 targetPosition) { }
    }
}
