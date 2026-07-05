using UnityEngine;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class StatusIndicatorView : MonoBehaviour
    {
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _runningColor;
        [SerializeField] private Color _stopColor;
        [SerializeField] private Color _failedColor;
        [SerializeField] private Color _completeColor;

        [SerializeField] private Image _image;

        public void SetBaseColor()
        {
            _image.color = _baseColor;
        }

        public void SetRunningColor()
        {
            _image.color = _runningColor;
        }

        public void SetStopColor()
        {
            _image.color = _stopColor;
        }

        public void SetFailedColor()
        {
            _image.color = _failedColor;
        }

        public void SetCompleteColor()
        {
            _image.color = _completeColor;
        }
    }
}