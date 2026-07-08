using UnityEditor;
using UnityEngine;

namespace RoboRoutine
{
    [RequireComponent(typeof(GridEntityAuthoring))]
    public sealed class GridEntityFootprintDrawer
    {
        private static readonly Color _fillColor = new Color(0f, 0.7f, 1f, 0.1f);
        private static readonly Color _fillColorSelected = new Color(0f, 0.7f, 1f, 0.4f);
        private static readonly Color _outlineColor = new Color(0f, 1f, 1f, 0.2f);
        private static readonly Color _outlineColorSelected = new Color(0f, 1f, 1f, 0.5f);

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected, typeof(GridEntityAuthoring))]
        private static void DrawFootprintGizmo(GridEntityAuthoring authoring, GizmoType gizmoType)
        {
            Vector2Int size = authoring.GetActualSize();
            Vector2Int origin = authoring.GetOrigin();


            Vector3 footprintPosition = new Vector3(origin.x, authoring.transform.position.y, origin.y);

            Color fillColor = _fillColor;
            Color outlineColor = _outlineColor;
            if ((gizmoType & GizmoType.Selected) != 0)
            {

                fillColor = _fillColorSelected;
                outlineColor = _outlineColorSelected;
            }

            DrawCells(size, footprintPosition, fillColor, outlineColor);
        }

        private static void DrawCells(Vector2Int size, Vector3 position, Color fillColor, Color outlineColor)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.y; z++)
                {
                    Vector3 cellCenter = new Vector3(position.x + x + 0.5f, position.y, position.z + z + 0.5f);

                    Gizmos.color = fillColor;
                    Gizmos.DrawCube(cellCenter, new Vector3(1f, 0.01f, 1f));

                    Gizmos.color = outlineColor;
                    Gizmos.DrawWireCube(cellCenter, new Vector3(1f, 0.01f, 1f));
                }
            }
        }
    }
}
