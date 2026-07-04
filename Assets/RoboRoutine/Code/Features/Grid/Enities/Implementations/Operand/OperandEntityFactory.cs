namespace RoboRoutine
{
    public sealed class OperandEntityFactory : GridEntityFactoryBase<NumericEntityModel, NumericEntityView, NumericEntityAuthoring>
    {
        public override EntityType EntityType => EntityType.Operand;

        public override IGridEntityController CreateController(NumericEntityAuthoring authoring, NumericEntityView view)
        {
            NumericEntityModel model = new NumericEntityModel(authoring.GetActualSize(), authoring.EntityType, authoring.Value);
            return new OperandEntityController(model, view);
        }
    }
}