using UnityEngine;

namespace RoboRoutine.Features
{
    public interface IGridView
    {
        Transform ItemRoot { get; }
        void Build(RectInt rect);
        Vector3 GetPosition();
    }
}
