using UnityEngine;

namespace RoboRoutine.Features
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class GridView : MonoBehaviour, IGridView
    {
        [SerializeField] private MeshFilter _meshFilter;

        public Transform ItemRoot => transform;

        public void Build(RectInt rect)
        {
            Vector2Int size = rect.size;
            Vector3 position = new Vector3(rect.position.x, transform.position.y, rect.position.y);

            int linesCount = size.x + size.y + 2;
            Vector3[] verts = new Vector3[linesCount * 2];
            int[] vertIndices = new int[linesCount * 2];

            int idx = 0;

            for (int x = 0; x < size.x + 1; x++)
            {
                verts[idx] = position + new Vector3(x, 0f, 0f);
                verts[idx + 1] = position + new Vector3(x, 0f, size.y);
                vertIndices[idx] = idx;
                vertIndices[idx + 1] = idx + 1;
                idx += 2;
            }

            for (int z = 0; z < size.y + 1; z++)
            {
                verts[idx] = position + new Vector3(0f, 0f, z);
                verts[idx + 1] = position + new Vector3(size.x, 0f, z);
                vertIndices[idx] = idx;
                vertIndices[idx + 1] = idx + 1;
                idx += 2;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = verts;
            mesh.SetIndices(vertIndices, MeshTopology.Lines, 0);
            mesh.RecalculateBounds();

            _meshFilter.mesh = mesh;
        }

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
    }
}
