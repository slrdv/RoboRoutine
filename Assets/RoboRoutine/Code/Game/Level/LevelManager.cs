namespace RoboRoutine
{
    public sealed class LevelManager : ILevelManager
    {
        private readonly IStateMachine _stateMachine;
        private readonly LevelRegistry _levelRegistry;

        private int _currentLevel = 0;

        public LevelManager(IStateMachine stateMachine, LevelRegistry levelRegistry)
        {
            _stateMachine = stateMachine;
            _levelRegistry = levelRegistry;
        }

        public void RunLevel(int level)
        {
            _currentLevel = level;
            _stateMachine.ChangeState<RunLevelState>();
        }

        public void LoadMenu()
        {
            LevelConfig config = _levelRegistry.Get(0);
            _stateMachine.ChangeState<LoadLevelState, LevelConfig>(config);
        }

        public void OnLoadLevelComplete()
        {
            RunLevel(_currentLevel);
        }

        public void OnLevelComplete(LevelResult result)
        {
            LoadNext();
        }

        private void LoadNext()
        {
            if (_levelRegistry.HasNextLevel(_currentLevel))
            {
                LevelConfig config = _levelRegistry.GetNext(_currentLevel);
                _stateMachine.ChangeState<LoadLevelState, LevelConfig>(config);
            }
            else
            {
                LoadMenu();
            }
        }
    }
}
