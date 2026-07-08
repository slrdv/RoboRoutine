namespace RoboRoutine
{
    public sealed class OperandEntityFactory : GridEntityFactoryBase<NumericEntityModel, NumericEntityView, NumericEntityAuthoring>
    {
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        public override EntityType EntityType => EntityType.Operand;

        public OperandEntityFactory(ISnapshotableRegistry snapshotableRegistry)
        {
            _snapshotableRegistry = snapshotableRegistry;
        }

        public override IGridEntityController CreateController(NumericEntityAuthoring authoring, NumericEntityView view)
        {
            NumericEntityModel model = new NumericEntityModel(authoring.GetActualSize(), authoring.EntityType, authoring.Value);
            return new OperandEntityController(model, view, _snapshotableRegistry);
        }
    }
}
