using System;

namespace RoboRoutine
{
    public sealed class SimulationPanelPresenter : IDisposable
    {
        private readonly SimulationPanelView _view;
        private readonly SimulationService _simulationService;

        public SimulationPanelPresenter(SimulationPanelView view, SimulationService simulationService)
        {
            _view = view;
            _simulationService = simulationService;

            _view.StartButton.PressedEvent += OnStartButtonPressed;
            _view.StopButton.PressedEvent += OnStopButtonPressed;
            _view.NextButton.PressedEvent += OnNextButtonPressed;
            _view.BackButton.PressedEvent += OnBackButtonPressed;

            _simulationService.SimulationRunEvent += OnSimulationStart;
            _simulationService.SimulationStopEvent += OnSimulationStop;
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
            _simulationService.SimulationStopEvent -= OnSimulationStop;
            _simulationService.SimulationRunEvent -= OnSimulationStart;

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
            _view.StartButton.SetEnabled(false);
            _view.StopButton.SetEnabled(_simulationService.CanStop());
            _view.NextButton.SetEnabled(false);
            _view.BackButton.SetEnabled(false);
        }

        private void OnSimulationStop()
        {
            UpdateUI();
        }
    }
}