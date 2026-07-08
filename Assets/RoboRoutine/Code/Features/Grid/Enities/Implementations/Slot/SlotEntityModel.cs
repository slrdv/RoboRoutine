using UnityEngine;

namespace RoboRoutine
{
    public sealed class SlotEntityModel : NumericEntityModel
    {
        private bool _isActivated;

        public bool IsActivated => _isActivated;

        public SlotEntityModel(Vector2Int size, EntityType entityType, int value) : base(size, EntityType.Slot, value) { }

        public void SetActivated(bool value)
        {
            _isActivated = value;
        }
    }
}