namespace RoboRoutine
{
    public sealed class StatusIndicatorPresenter
    {
        private readonly StatusIndicatorView _view;
        private readonly ISimulationState _simulation;

        public StatusIndicatorPresenter(StatusIndicatorView view, ISimulationState simulation)
        {
            _view = view;
            _simulation = simulation;

            _simulation.StartEvent += OnSimulationStart;
            _simulation.StopEvent += OnSimulationStop;
            _simulation.ResetEvent += OnSimulationReset;
            _simulation.FailedEvent += OnSimulationFailed;

            _view.SetBaseColor();
        }

        private void OnSimulationStart()
        {
            _view.SetRunningColor();
        }

        private void OnSimulationStop()
        {
            _view.SetStopColor();
        }

        private void OnSimulationReset()
        {
            _view.SetBaseColor();
        }

        private void OnSimulationFailed()
        {
            _view.SetFailedColor();
        }

        private void OnSimulationComplete()
        {
            _view.SetCompleteColor();
        }
    }
}
