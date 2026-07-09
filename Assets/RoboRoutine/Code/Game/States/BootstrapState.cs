using Cysharp.Threading.Tasks;

namespace RoboRoutine
{
    public sealed class BootstrapState : StateEntryBase
    {
        private readonly ILevelManager _levelManager;
        private readonly ILoadingScreen _loadingScreen;
        private readonly IRepository<CommandType, CommandConfig> _commandConfigRepository;
        private readonly IRepository<int, LevelConfig> _levelConfigRepository;

        public BootstrapState(
            ILevelManager levelManager,
            ILoadingScreen loadingScreen,
            IRepository<CommandType, CommandConfig> commandConfigRepository,
            IRepository<int, LevelConfig> levelConfigRepository)
        {
            _levelManager = levelManager;
            _loadingScreen = loadingScreen;
            _commandConfigRepository = commandConfigRepository;
            _levelConfigRepository = levelConfigRepository;
        }

        public override async UniTask Enter()
        {
            await _loadingScreen.Show();

            _commandConfigRepository.Load();
            _levelConfigRepository.Load();

            _levelManager.LoadMenu();

            Complete();
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
