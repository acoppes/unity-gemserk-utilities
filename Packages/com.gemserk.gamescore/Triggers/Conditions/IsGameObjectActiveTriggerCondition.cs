using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers.Conditions
{
    public class IsGameObjectActiveTriggerCondition : TriggerCondition
    {
        public GameObject gameObject;
        
        public override string GetObjectName()
        {
            if (!gameObject)
            {
                return $"IsGameObjectActive(activator)";
            }
            return $"IsGameObjectActive({gameObject.name})";
        }

        public override bool Evaluate(object activator = null)
        {
            if (gameObject)
            {
                return gameObject.activeSelf;
            }
            
            if (activator is GameObject activatorObject)
            {
                return activatorObject.activeSelf;
            }

            return false;
        }
    }
}