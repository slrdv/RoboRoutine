using UnityEngine;

namespace RoboRoutine
{
    public class GridEntityView : MonoBehaviour
    {
        [SerializeField] private float _transparency = 0.2f;
        [SerializeField] private Renderer _renderer;

        private float _originAlpha;

        public void SetTransparent(bool value)
        {
            Color color = _renderer.material.color;
            color.a = value ? _transparency : _originAlpha;
        }

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

        private void Awake()
        {
            _originAlpha = _renderer.material.color.a;
        }
    }
}