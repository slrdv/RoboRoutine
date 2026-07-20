using UnityEngine;

namespace RoboRoutine.UI
{
    public sealed class SimulationPanelView : MonoBehaviour
    {
        [SerializeField] private PanelButton _startButton;
        [SerializeField] private PanelButton _resetButton;
        [SerializeField] private PanelButton _nextButton;
        [SerializeField] private PanelButton _undoButton;

        public PanelButton StartButton => _startButton;
        public PanelButton ResetButton => _resetButton;
        public PanelButton NextButton => _nextButton;
        public PanelButton UndoButton => _undoButton;
    }
}
