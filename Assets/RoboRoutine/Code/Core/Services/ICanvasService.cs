using UnityEngine;

namespace RoboRoutine.Core
{
    public interface ICanvasService
    {
        Vector2 GetScreenPosition(RectTransform rectTransform, bool forceUpdate = false);
        Vector2 GetLocalPosition(RectTransform parentRect, Vector2 screenPosition, bool forceUpdate = false);
        void SetScreenPosition(RectTransform rectTransform, Vector2 screenPosition);
        bool IsPointOverRect(RectTransform rect, Vector2 screenPosition, bool forceUpdate = false);
        void ForceUpdate();
    }
}
