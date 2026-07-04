namespace RoboRoutine
{
    public sealed class OperandEntityController : GridEntityController<NumericEntityModel, NumericEntityView>
    {
        
        public OperandEntityController(NumericEntityModel model, NumericEntityView view) : base(model, view)
        {
            view.SetLabel(model.Value);
        }
    }
}