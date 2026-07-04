using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridState
    {
        public Dictionary<Vector2Int, IGridEntityController> Entities;
    }
}