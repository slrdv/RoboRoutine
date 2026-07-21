using UnityEngine;

namespace RoboRoutine.Features
{
    public class GridEntityView : MonoBehaviour, IGridEntityView
    {
        private Quaternion _originRotation;

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }

        public void SetScale(Vector3 scale)
        {
            gameObject.transform.localScale = scale;
        }

        public void SetParent(Transform parent)
        {
            gameObject.transform.SetParent(parent);
        }

        public void UpdateRotation()
        {
            gameObject.transform.rotation = _originRotation;
        }

        private void Awake()
        {
            _originRotation = gameObject.transform.rotation;
        }
    }
}
