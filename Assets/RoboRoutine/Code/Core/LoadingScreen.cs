using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _fillBar;
        [SerializeField] private TMP_Text _progressText;

        private CancellationTokenSource _cts;

        public async UniTask Show()
        {
            await Show(0f);
        }

        public async UniTask Show(float fadeDuration)
        {
            gameObject.SetActive(true);
            await FadeAsync(0f, 1f, fadeDuration);
        }

        public async UniTask Hide()
        {
            await Hide(0f);
        }

        public async UniTask Hide(float fadeDuration)
        {
            await FadeAsync(1f, 0f, fadeDuration);
            gameObject.SetActive(false);
        }

        public void SetProgress(float progress, string text = null)
        {
            _fillBar.fillAmount = Mathf.Clamp01(progress);
            if (text != null)
            {
                _progressText.text = text;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTask FadeAsync(float from, float to, float seconds)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            await ChangeAlphaAsync(from, to, seconds, _cts.Token);
        }

        private async UniTask ChangeAlphaAsync(float from, float to, float seconds, CancellationToken cancellationToken = default)
        {
            float elapsed = 0f;
            SetAlpha(from);

            while (elapsed < seconds && !cancellationToken.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                SetAlpha(Mathf.Lerp(from, to, elapsed / seconds));
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();
            }

            SetAlpha(to);
        }

        private void SetAlpha(float value)
        {
            if (_canvasGroup == null) return;

            _canvasGroup.alpha = value;
        }
    }
}
