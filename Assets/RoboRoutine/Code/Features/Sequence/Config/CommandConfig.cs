using RoboRoutine.Core;
using UnityEngine;

namespace RoboRoutine.Features
{
    [CreateAssetMenu(fileName = "CommandConfig", menuName = "Configs/Commands/CommandConfig")]
    public class CommandConfig : ScriptableObject, IHasKey<CommandType>
    {
        [field: SerializeField] public CommandType Type { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public CommandType Key => Type;
    }
}
