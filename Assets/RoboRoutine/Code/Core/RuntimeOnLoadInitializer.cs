using R3;
using UnityEngine;

namespace RoboRoutine
{
    public static class RuntimeOnLoadInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            ObservableSystem.RegisterUnhandledExceptionHandler(ex => { Debug.LogException(ex); });
        }
    }
}