using System;

namespace RoboRoutine
{
    public sealed class OperandEntityController : GridEntityController<NumericEntityModel, NumericEntityView>, ISnapshotable, IDisposable
    {
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        public int Value => _model.Value;

        public SnapshotLayer SnapshotLayer => SnapshotLayer.Operand;

        public OperandEntityController(NumericEntityModel model, NumericEntityView view, ISnapshotableRegistry snapshotableRegistry) : base(model, view)
        {
            _snapshotableRegistry = snapshotableRegistry;
            _snapshotableRegistry.Register(this);

            view.SetLabel(model.Value);
        }

        public void SetValue(int value)
        {
            _model.SetValue(value);
            _view.SetLabel(_model.Value);
        }

        public object CaptureState()
        {
            return new OperandHistoryState { Value = _model.Value };
        }
        public void RestoreState(object state)
        {
            if (state is not OperandHistoryState operandState) throw new ArgumentException($"Invalid history state type: {state.GetType().Name}");
            SetValue(operandState.Value);
        }

        public void Dispose()
        {
            _snapshotableRegistry.Remove(this);
        }
    }
}
