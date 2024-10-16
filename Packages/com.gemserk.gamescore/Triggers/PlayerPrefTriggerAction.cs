using Gemserk.Triggers;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class PlayerPrefTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            SetInt = 0,
            SetFloat = 1,
            SetString = 2,
            Delete = 3
        }

        public ActionType actionType;
        public string preferenceKey;
        
        [ConditionalField(nameof(actionType), false, ActionType.SetInt)]
        public int intValue;
        [ConditionalField(nameof(actionType), false, ActionType.SetFloat)]
        public float floatValue;
        [ConditionalField(nameof(actionType), false, ActionType.SetString)]
        public string stringValue;
        
        public override string GetObjectName()
        {
            return $"{actionType}({preferenceKey})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (actionType == ActionType.SetInt)
            {
                PlayerPrefs.SetInt(preferenceKey, intValue);
            } else if (actionType == ActionType.SetFloat)
            {
                PlayerPrefs.SetFloat(preferenceKey, floatValue);
            } else if (actionType == ActionType.SetString)
            {
                PlayerPrefs.SetString(preferenceKey, stringValue);
            } else if (actionType == ActionType.Delete)
            {
                PlayerPrefs.DeleteKey(preferenceKey);
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}