using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class PlayerPrefTriggerCondition : TriggerCondition
    {
        public enum ConditionType
        {
            HasPref = 0
        }

        public ConditionType conditionType;
        public string preferenceKey;
        
        public override string GetObjectName()
        {
            return $"{conditionType}({preferenceKey})";
        }

        public override bool Evaluate(object activator = null)
        {
            if (conditionType == ConditionType.HasPref)
            {
                return PlayerPrefs.HasKey(preferenceKey);
            }

            return false;
        }
    }
}