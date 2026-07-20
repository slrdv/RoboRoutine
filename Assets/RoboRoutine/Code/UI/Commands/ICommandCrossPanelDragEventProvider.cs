using System;
using RoboRoutine.Features;
using UnityEngine.EventSystems;

namespace RoboRoutine.UI
{
    public interface ICommandCrossPanelDragEventProvider
    {
        event Action BeginDragEvent;
        event Action<PointerEventData> DragEvent;
        event Action<CommandData> EndDragEvent;
    }
}
