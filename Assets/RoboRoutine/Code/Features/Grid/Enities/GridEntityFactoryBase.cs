using System.IO;

namespace RoboRoutine.Features
{
    public abstract class GridEntityFactoryBase<TModel, TView, TAuthoring> : IGridEntityFactory
        where TModel : GridEntityModel
        where TView : GridEntityView
        where TAuthoring : GridEntityAuthoring
    {
        public abstract EntityType EntityType { get; }

        public IGridEntityController Create(GridEntityAuthoring authoring)
        {
            if (authoring is not TAuthoring typed)
                throw new InvalidDataException($"Wrong authoring type {authoring.GetType().Name}, {typeof(TAuthoring).Name} expected");

            TView view = typed.GetComponentInChildren<TView>();
            if (view == null) throw new InvalidDataException($"Component {typeof(TView).Name} is expected");

            return CreateController(typed, view);
        }

        public abstract IGridEntityController CreateController(TAuthoring authoring, TView view);
    }
}
