using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine.Tests
{
    public sealed class TestGridView : IGridView
    {
        public Transform ItemRoot => null;

        public void Build(RectInt rect) { }
        public Vector3 GetPosition()
        {
            return Vector3.zero;
        }
    }
}
