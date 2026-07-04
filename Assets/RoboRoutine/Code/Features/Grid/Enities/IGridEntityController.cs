using UnityEngine;

namespace RoboRoutine
{
    public interface IGridEntityController
    {
        GridEntityModel Model { get; }
        GridEntityView View { get; }
        EntityType EntityType { get; }

        void SetPosition(Vector3 position);
    }
}