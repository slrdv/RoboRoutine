using Cysharp.Threading.Tasks;
using RoboRoutine.Core;


namespace RoboRoutine.Game
{
    public sealed class RunLevelState : StateEntryBase
    {
        private readonly ILoadingScreen _loadingScreen;

        public RunLevelState(ILoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public override async UniTask Enter()
        {
            await _loadingScreen.Hide(0.5f);
            Complete();
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
