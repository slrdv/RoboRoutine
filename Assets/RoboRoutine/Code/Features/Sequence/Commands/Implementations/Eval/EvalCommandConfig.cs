using System;
using UnityEngine;

namespace RoboRoutine
{
    [CreateAssetMenu(fileName = "EvalCommandConfig", menuName = "Configs/EvalCommandConfig")]
    public sealed class EvalCommandConfig : CommandConfig
    {
        [field: SerializeField] public SerializablePair<EvalType, ItemConfig>[] ItemsData;

        public ItemConfig GetItemConfig(EvalType type)
        {
            for (int i = 0; i < ItemsData.Length; i++)
            {
                SerializablePair<EvalType, ItemConfig> kv = ItemsData[i];
                if (kv.Key == type)
                {
                    return kv.Value;
                }
            }

            return new ItemConfig { Icon = Icon, DisplayName = DisplayName };
        }

        [Serializable]
        public struct ItemConfig
        {
            public Sprite Icon;
            public string DisplayName;
        }
    }
}