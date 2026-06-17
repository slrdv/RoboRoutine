using UnityEngine;

namespace RoboRoutine
{
    public sealed class SimulationPanelView : MonoBehaviour
    {
        [SerializeField] private PanelButton _startButton;
        [SerializeField] private PanelButton _stopButton;
        [SerializeField] private PanelButton _nextButton;
        [SerializeField] private PanelButton _backButton;

        public PanelButton StartButton => _startButton;
        public PanelButton StopButton => _stopButton;
        public PanelButton NextButton => _nextButton;
        public PanelButton BackButton => _backButton;
    }
}