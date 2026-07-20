using Cysharp.Threading.Tasks;

namespace RoboRoutine.Core
{
    public interface ILoadingScreen
    {
        UniTask Show();
        UniTask Show(float fadeDuration);
        UniTask Hide();
        UniTask Hide(float fadeDuration);

        void SetProgress(float progress, string text = null);
    }
}
