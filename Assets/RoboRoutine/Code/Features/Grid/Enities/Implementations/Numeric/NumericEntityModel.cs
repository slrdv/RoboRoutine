using UnityEngine;

namespace RoboRoutine
{
    public class NumericEntityModel : GridEntityModel
    {
        private int _value;

        public int Value => _value;

        public NumericEntityModel(Vector2Int size, EntityType entityType, int value) : base(size, entityType)
        {
            _value = value;
        }

        public void SetValue(int value)
        {
            _value = value;
        }
    }
}
