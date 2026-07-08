using UnityEngine;

namespace RoboRoutine
{
    [CreateAssetMenu(fileName = "ConditionCommandConfig", menuName = "Configs/ConditionCommandConfig")]
    public sealed class ConditionCommandConfig : CommandConfig
    {
        [field: SerializeField] public SerializablePair<ConditionType, string>[] ItemsData;

        public string GetLabel(ConditionType type)
        {
            for (int i = 0; i < ItemsData.Length; i++)
            {
                SerializablePair<ConditionType, string> kv = ItemsData[i];
                if (kv.Key == type)
                {
                    return kv.Value;
                }
            }

            return DisplayName;
        }
    }
}
