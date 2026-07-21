using UnityEngine;

namespace RoboRoutine.Features
{
    public interface IRobotView
    {
        Transform ItemRoot { get; }
        Vector3 ItemScale { get; }

        Vector2 GetPositionXZ();
        Quaternion GetRotation();
        void LookAtXZ(Vector2 targetPosition);
        void MoveTowardsXZ(Vector2 targetPosition, float delta);
        void SetPositionXZ(Vector2 position);
        void SetRotation(Quaternion rotation);
    }
}
