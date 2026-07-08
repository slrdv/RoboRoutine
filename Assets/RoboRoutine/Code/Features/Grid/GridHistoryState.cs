using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridHistoryState
    {
        public Dictionary<Vector2Int, IGridEntityController> Entities;
    }
}
