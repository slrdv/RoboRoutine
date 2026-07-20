using RoboRoutine.Features;
using UnityEngine.EventSystems;

namespace RoboRoutine.UI
{
    public interface ICommandCrossPanelDrag
    {
        void BeginDrag();
        void Drag(PointerEventData eventData);
        void EndDrag(CommandData commandData);
    }
}
