namespace RoboRoutine.Features
{
    public sealed class SlotEntityFactory : GridEntityFactoryBase<NumericEntityModel, INumericEntityView, NumericEntityAuthoring>
    {
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        public override EntityType EntityType => EntityType.Slot;

        public SlotEntityFactory(ISnapshotableRegistry snapshotableRegistry)
        {
            _snapshotableRegistry = snapshotableRegistry;
        }

        public override IGridEntityController CreateController(NumericEntityAuthoring authoring, INumericEntityView view)
        {
            SlotEntityModel model = new SlotEntityModel(authoring.GetActualSize(), authoring.EntityType, authoring.Value);
            return new SlotEntityController(model, view, _snapshotableRegistry);
        }
    }
}
