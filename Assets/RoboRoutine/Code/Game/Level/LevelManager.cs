using UnityEngine;

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

        public void LoadMenu()
        {
            LoadLevel(0);
        }

        public void LoadLevel(int level)
        {
            Debug.Log($"Loading level {level}");

            LevelConfig config = _levelRegistry.Get(level);
            _stateMachine.ChangeState<LoadLevelState, LevelConfig>(config);
        }

        public void LoadNext()
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

        public void OnLoadLevelComplete()
        {
            RunLevel(_currentLevel);
        }

        public void OnLevelComplete(LevelResult result)
        {
            LoadNext();
        }

        private void RunLevel(int level)
        {
            _currentLevel = level;
            _stateMachine.ChangeState<RunLevelState>();
        }
    }
}
