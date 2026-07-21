namespace RoboRoutine.Features
{
    public sealed class OperandEntityFactory : GridEntityFactoryBase<NumericEntityModel, INumericEntityView, NumericEntityAuthoring>
    {
        private readonly ISnapshotableRegistry _snapshotableRegistry;

        public override EntityType EntityType => EntityType.Operand;

        public OperandEntityFactory(ISnapshotableRegistry snapshotableRegistry)
        {
            _snapshotableRegistry = snapshotableRegistry;
        }

        public override IGridEntityController CreateController(NumericEntityAuthoring authoring, INumericEntityView view)
        {
            NumericEntityModel model = new NumericEntityModel(authoring.GetActualSize(), authoring.EntityType, authoring.Value);
            return new OperandEntityController(model, view, _snapshotableRegistry);
        }
    }
}
