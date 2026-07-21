using UnityEngine;

namespace RoboRoutine.Features
{
    public interface IGridEntityView
    {
        Vector3 GetPosition();
        void SetParent(Transform parent);
        void SetPosition(Vector3 position);
        void SetScale(Vector3 scale);
        void UpdateRotation();
    }
}
