using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine.Features
{
    public sealed class GridHistoryState
    {
        public Dictionary<Vector2Int, IGridEntityController> Entities;
    }
}
