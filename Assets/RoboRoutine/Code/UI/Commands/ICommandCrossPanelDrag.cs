using UnityEngine.EventSystems;

namespace RoboRoutine
{
    public interface ICommandCrossPanelDrag
    {
        void BeginDrag();
        void Drag(PointerEventData eventData);
        void EndDrag(CommandData commandData);
    }
}