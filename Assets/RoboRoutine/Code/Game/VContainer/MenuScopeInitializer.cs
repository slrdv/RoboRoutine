using VContainer.Unity;

namespace RoboRoutine
{
    public sealed class MenuScopeInitializer : IStartable
    {
        private readonly ILevelManager _levelManager;

        public MenuScopeInitializer(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Start()
        {
            _levelManager.OnLoadLevelComplete();
        }
    }
}
