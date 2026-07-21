namespace RoboRoutine.Features
{
    public sealed class ObstacleEntityFactory : GridEntityFactoryBase<GridEntityModel, IGridEntityView, GridEntityAuthoring>
    {
        public override EntityType EntityType => EntityType.Obstacle;

        public override IGridEntityController CreateController(GridEntityAuthoring authoring, IGridEntityView view)
        {
            GridEntityModel model = new GridEntityModel(authoring.GetActualSize(), authoring.EntityType);
            return new GridEntityController<GridEntityModel, IGridEntityView>(model, view);
        }
    }
}
