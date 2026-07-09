using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace RoboRoutine
{
    public sealed class LoadLevelState : StateEntryBase<LevelConfig>
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IStorage<string, SequenceData> _sequenceStorage;

        public LoadLevelState(ILoadingScreen loadingScreen, IStorage<string, SequenceData> sequenceStorage)
        {
            _loadingScreen = loadingScreen;
            _sequenceStorage = sequenceStorage;
        }

        public override async UniTask Enter(LevelConfig config)
        {
            await _loadingScreen.Show();

            _sequenceStorage.Initialize($"{Constants.ProjectName}_level_{config.LevelNum}");
            _sequenceStorage.Load();

            await SceneManager.LoadSceneAsync(config.SceneName, LoadSceneMode.Single).ToUniTask();

            Complete();
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
