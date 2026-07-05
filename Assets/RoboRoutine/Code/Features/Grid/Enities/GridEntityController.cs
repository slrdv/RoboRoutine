using UnityEngine;

namespace RoboRoutine
{
    public class GridEntityController<TModel, TView> : IGridEntityController where TView : GridEntityView where TModel : GridEntityModel
    {
        private readonly TModel _model;
        private readonly TView _view;

        public GridEntityModel Model => _model;
        public GridEntityView View => _view;
        public EntityType EntityType => _model.EntityType;

        public GridEntityController(TModel model, TView view)
        {
            _model = model;
            _view = view;
        }

        public void AttachToParent(Transform parent, Vector3 position, Vector3 scale)
        {
            _view.SetParent(parent);
            _view.SetScale(scale);
            _view.SetPosition(position);
        }
    }
}