using UnityEngine;

namespace RoboRoutine
{
    public interface IGridEntityController
    {
        GridEntityModel Model { get; }
        GridEntityView View { get; }
        EntityType EntityType { get; }

        void AttachToParent(Transform parent, Vector3 position, Vector3 scale);
    }
}