using UnityEngine;

namespace RoboRoutine
{
    public class NumericEntityAuthoring : GridEntityAuthoring
    {
        [SerializeField] private int _value;

        public int Value => _value;
    }
}