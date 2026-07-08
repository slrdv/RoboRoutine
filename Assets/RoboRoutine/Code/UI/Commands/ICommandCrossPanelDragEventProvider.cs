using System;
using UnityEngine.EventSystems;

namespace RoboRoutine
{
    public interface ICommandCrossPanelDragEventProvider
    {
        event Action BeginDragEvent;
        event Action<PointerEventData> DragEvent;
        event Action<CommandData> EndDragEvent;
    }
}
