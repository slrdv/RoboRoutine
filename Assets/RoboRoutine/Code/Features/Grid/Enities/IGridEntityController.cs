using UnityEngine;

namespace RoboRoutine.Features
{
    public interface IGridEntityController
    {
        GridEntityModel Model { get; }
        IGridEntityView View { get; }
        EntityType EntityType { get; }

        void AttachToParent(Transform parent, Vector3 position, Vector3 scale);
        void UpdateRotation();
    }
}
