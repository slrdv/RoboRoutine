namespace RoboRoutine.Features
{
    public sealed class ObstacleEntityFactory : GridEntityFactoryBase<GridEntityModel, GridEntityView, GridEntityAuthoring>
    {
        public override EntityType EntityType => EntityType.Obstacle;

        public override IGridEntityController CreateController(GridEntityAuthoring authoring, GridEntityView view)
        {
            GridEntityModel model = new GridEntityModel(authoring.GetActualSize(), authoring.EntityType);
            return new GridEntityController<GridEntityModel, GridEntityView>(model, view);
        }
    }
}
