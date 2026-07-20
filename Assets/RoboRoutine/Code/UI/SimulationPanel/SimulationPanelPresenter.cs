using System;
using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class SimulationPanelPresenter : IDisposable
    {
        private readonly SimulationPanelView _view;
        private readonly ISimulationService _simulationService;
        private readonly ISimulationState _simulation;

        public SimulationPanelPresenter(SimulationPanelView view, ISimulationService simulationService, ISimulationState simulation)
        {
            _view = view;
            _simulationService = simulationService;
            _simulation = simulation;
            _view.StartButton.PressedEvent += OnStartButtonPressed;
            _view.ResetButton.PressedEvent += OnStopButtonPressed;
            _view.NextButton.PressedEvent += OnNextButtonPressed;
            _view.UndoButton.PressedEvent += OnUndoButtonPressed;

            _simulation.StartEvent += OnSimulationStart;
            _simulation.StopEvent += OnSimulationStop;
            _simulation.ResetEvent += OnSimulationReset;
            _simulation.FailedEvent += OnSimulationFailed;
        }

        public void UpdateUI()
        {
            _view.StartButton.SetEnabled(_simulationService.CanRunNext());
            _view.ResetButton.SetEnabled(_simulationService.CanReset());
            _view.NextButton.SetEnabled(_simulationService.CanRunNext());
            _view.UndoButton.SetEnabled(_simulationService.CanUndo());
        }

        public void Dispose()
        {
            _simulation.StartEvent -= OnSimulationStart;
            _simulation.StopEvent -= OnSimulationStop;
            _simulation.ResetEvent -= OnSimulationReset;
            _simulation.FailedEvent -= OnSimulationFailed;

            _view.StartButton.PressedEvent -= OnStartButtonPressed;
            _view.ResetButton.PressedEvent -= OnStopButtonPressed;
            _view.NextButton.PressedEvent -= OnNextButtonPressed;
            _view.UndoButton.PressedEvent -= OnUndoButtonPressed;
        }

        private void OnStartButtonPressed()
        {
            _simulationService.RunAll();
        }

        private void OnStopButtonPressed()
        {
            _simulationService.Reset();
        }

        private void OnNextButtonPressed()
        {
            _simulationService.RunNext();
        }

        private void OnUndoButtonPressed()
        {
            _simulationService.Undo();
        }


        private void OnSimulationStart()
        {
            DisableAllButtons();
            _view.ResetButton.SetEnabled(true);
        }

        private void OnSimulationStop()
        {
            UpdateUI();
        }

        private void OnSimulationReset()
        {
            UpdateUI();
        }

        private void OnSimulationFailed()
        {
            DisableAllButtons();
            _view.ResetButton.SetEnabled(true);
        }

        private void DisableAllButtons()
        {
            _view.StartButton.SetEnabled(false);
            _view.ResetButton.SetEnabled(false);
            _view.NextButton.SetEnabled(false);
            _view.UndoButton.SetEnabled(false);
        }
    }
}
