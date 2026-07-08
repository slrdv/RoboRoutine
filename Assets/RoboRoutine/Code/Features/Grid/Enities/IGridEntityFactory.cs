namespace RoboRoutine
{
    public interface IGridEntityFactory
    {
        EntityType EntityType { get; }

        IGridEntityController Create(GridEntityAuthoring authoring);
    }
}
