using System;

namespace RoboRoutine
{
    public sealed class SimulationPanelPresenter : IDisposable
    {
        private readonly SimulationPanelView _view;
        private readonly ISimulationService _simulationService;
        private readonly ISimulationState _simulationState;

        public SimulationPanelPresenter(SimulationPanelView view, ISimulationService simulationService, ISimulationState simulationState)
        {
            _view = view;
            _simulationService = simulationService;
            _simulationState = simulationState;
            _view.StartButton.PressedEvent += OnStartButtonPressed;
            _view.StopButton.PressedEvent += OnStopButtonPressed;
            _view.NextButton.PressedEvent += OnNextButtonPressed;
            _view.BackButton.PressedEvent += OnBackButtonPressed;

            _simulationState.SimulationRunEvent += OnSimulationStart;
            _simulationState.SimulationStopEvent += OnSimulationStop;
        }

        public void UpdateUI()
        {
            _view.StartButton.SetEnabled(_simulationService.CanRunNext());
            _view.StopButton.SetEnabled(_simulationService.CanStop());
            _view.NextButton.SetEnabled(_simulationService.CanRunNext());
            _view.BackButton.SetEnabled(_simulationService.CanBack());
        }

        public void Dispose()
        {
            _simulationState.SimulationStopEvent -= OnSimulationStop;
            _simulationState.SimulationRunEvent -= OnSimulationStart;

            _view.StartButton.PressedEvent -= OnStartButtonPressed;
            _view.StopButton.PressedEvent -= OnStopButtonPressed;
            _view.NextButton.PressedEvent -= OnNextButtonPressed;
            _view.BackButton.PressedEvent -= OnBackButtonPressed;
        }

        private void OnStartButtonPressed()
        {
            _simulationService.RunAll();
        }

        private void OnStopButtonPressed()
        {
            _simulationService.Stop();
        }

        private void OnNextButtonPressed()
        {
            _simulationService.RunNext();
        }

        private void OnBackButtonPressed()
        {
            _simulationService.Back();
        }

        private void OnSimulationStart()
        {
            UpdateUI();
        }

        private void OnSimulationStop()
        {
            UpdateUI();
        }
    }
}