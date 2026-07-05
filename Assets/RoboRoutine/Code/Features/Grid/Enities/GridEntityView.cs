using UnityEngine;

namespace RoboRoutine
{
    public class GridEntityView : MonoBehaviour
    {
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
    }
}