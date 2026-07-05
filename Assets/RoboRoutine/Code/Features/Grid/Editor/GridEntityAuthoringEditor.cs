using UnityEditor;
using UnityEngine;

namespace RoboRoutine
{
    [CustomEditor(typeof(GridEntityAuthoring), true)]
    public class GridEntitySnapperEditor : Editor
    {
        private GridEntityAuthoring _authoring;

        private void OnEnable()
        {
            _authoring = (GridEntityAuthoring)target;
        }

        private void OnSceneGUI()
        {
            if (EditorApplication.isPlaying) return;

            ApplyRotation();
            Snap();
        }

        private void Snap()
        {
            Vector2Int size = _authoring.GetActualSize();

            Vector3 worldPos = _authoring.transform.position;
            Vector3 centerOffset = new Vector3(size.x * 0.5f, 0f, size.y * 0.5f);
            Vector3 footprintPos = worldPos - centerOffset;

            Vector3 snappedPos = new Vector3(Mathf.Round(footprintPos.x), footprintPos.y, Mathf.Round(footprintPos.z)) + centerOffset;

            if (Vector3.Distance(worldPos, snappedPos) > 0.001f)
            {
                _authoring.transform.position = snappedPos;
            }
        }

        private void ApplyRotation()
        {
            _authoring.transform.rotation = Quaternion.Euler(0, (int)_authoring.Rotation * 90, 0);
        }
    }
}