namespace RoboRoutine.Features
{
    public interface IGridEntityFactory
    {
        EntityType EntityType { get; }

        IGridEntityController Create(GridEntityAuthoring authoring);
    }
}
