using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace RoboRoutine
{
    public sealed class InputHandler : MonoBehaviour, IInputHandler
    {
        public event Action IgmActionEvent;

        [SerializeField] private InputActionReference _igmAction;

        private void Awake()
        {
            _igmAction.action.Enable();
            _igmAction.action.performed += OnIgmAction;
        }

        private void OnDestroy()
        {
            _igmAction.action.performed -= OnIgmAction;
            _igmAction.action.Disable();
        }

        private void OnIgmAction(CallbackContext ctx)
        {
            IgmActionEvent?.Invoke();
        }
    }
}
