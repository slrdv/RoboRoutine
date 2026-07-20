using UnityEngine;

namespace RoboRoutine.Features
{
    public class NumericEntityAuthoring : GridEntityAuthoring
    {
        [SerializeField] private int _value;

        public int Value => _value;
    }
}
