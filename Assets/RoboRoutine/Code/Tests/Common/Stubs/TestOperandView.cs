using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine.Tests
{
    public sealed class TestOperandView : INumericEntityView
    {
        public Vector3 GetPosition()
        {
            return Vector3.zero;
        }

        public void SetLabel(int value) { }
        public void SetParent(Transform parent) { }
        public void SetPosition(Vector3 position) { }
        public void SetScale(Vector3 scale) { }
        public void UpdateRotation() { }
    }
}
