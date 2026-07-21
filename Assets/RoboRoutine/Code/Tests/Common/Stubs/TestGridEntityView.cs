using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine.Tests
{
    public sealed class TestGridEntityView : IGridEntityView
    {
        private Vector3 _position;

        public Vector3 GetPosition()
        {
            return _position;
        }

        public void SetParent(Transform parent) { }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public void SetScale(Vector3 scale) { }

        public void UpdateRotation() { }
    }
}
