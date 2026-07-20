using RoboRoutine.Core;
using UnityEngine;

namespace RoboRoutine.Game
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level/LevelConfig")]
    public sealed class LevelConfig : ScriptableObject, IHasKey<int>
    {
        [field: SerializeField] public int LevelNum;
        [field: SerializeField] public string SceneName;

        public int Key => LevelNum;
    }
}
