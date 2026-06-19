using System;
using UnityEngine.EventSystems;

namespace RoboRoutine
{
    public sealed class CommandCrossPanelDragService : ICommandCrossPanelDrag, ICommandCrossPanelDragEventProvider
    {
        public event Action BeginDragEvent;
        public event Action<PointerEventData> DragEvent;
        public event Action<CommandData> EndDragEvent;

        public void BeginDrag()
        {
            BeginDragEvent?.Invoke();
        }

        public void Drag(PointerEventData eventData)
        {
            DragEvent?.Invoke(eventData);
        }

        public void EndDrag(CommandData commandData)
        {
            EndDragEvent?.Invoke(commandData);
        }
    }
}