using System;

namespace RoboRoutine
{
    public sealed class SlotEntityController : GridEntityController<SlotEntityModel, NumericEntityView>, ISnapshotable, IDisposable
    {
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        public int Value => _model.Value;

        public SnapshotLayer SnapshotLayer => SnapshotLayer.Slot;

        public SlotEntityController(SlotEntityModel model, NumericEntityView view, ISnapshotableRegistry snapshotableRegistry) : base(model, view)
        {
            _snapshotableRegistry = snapshotableRegistry;
            _snapshotableRegistry.Register(this);

            view.SetLabel(model.Value);
        }

        public void SetActivated(bool activated)
        {
            _model.SetActivated(activated);
        }

        public object CaptureState()
        {
            return new SlotHistoryState { IsActivated = _model.IsActivated };
        }
        public void RestoreState(object state)
        {
            if (state is not SlotHistoryState operandState) throw new ArgumentException($"Invalid history state type: {state.GetType().Name}");

            SetActivated(operandState.IsActivated);
        }

        public void Dispose()
        {
            _snapshotableRegistry.Remove(this);
        }
    }
}