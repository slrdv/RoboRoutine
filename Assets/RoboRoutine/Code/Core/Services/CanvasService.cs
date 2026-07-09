using UnityEngine;

namespace RoboRoutine
{
    public sealed class CanvasService : ICanvasService
    {
        private readonly Canvas _canvas;
        private readonly Camera _camera;

        private Camera EventCamera => _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _camera;

        public CanvasService(Canvas canvas, Camera camera)
        {
            _canvas = canvas;
            _camera = camera;
        }

        public Vector2 GetScreenPosition(RectTransform rectTransform, bool forceUpdate = false)
        {
            if (forceUpdate)
            {
                ForceUpdate();
            }

            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return rectTransform.position;
            }

            return RectTransformUtility.WorldToScreenPoint(EventCamera, rectTransform.position);
        }

        public Vector2 GetLocalPosition(RectTransform parent, Vector2 screenPosition, bool forceUpdate = false)
        {
            if (forceUpdate)
            {
                ForceUpdate();
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPosition, EventCamera, out Vector2 localPoint))
            {
                return localPoint;
            }

            return default;
        }

        public void SetScreenPosition(RectTransform rectTransform, Vector2 screenPosition)
        {
            if (rectTransform.parent is RectTransform parent)
            {
                Vector2 localPos = GetLocalPosition(parent, screenPosition);
                rectTransform.anchoredPosition = localPos;
            }
        }

        public bool IsPointOverRect(RectTransform rect, Vector2 screenPosition, bool forceUpdate = false)
        {
            if (forceUpdate)
            {
                ForceUpdate();
            }

            return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPosition, EventCamera);
        }

        public void ForceUpdate()
        {
            Canvas.ForceUpdateCanvases();
        }
    }
}
