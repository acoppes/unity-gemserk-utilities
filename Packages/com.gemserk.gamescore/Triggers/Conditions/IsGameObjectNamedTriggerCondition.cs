using System;
using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers.Conditions
{
    public class IsGameObjectNamedTriggerCondition : TriggerCondition
    {
        public string expectedName;
        
        public override string GetObjectName()
        {
            return $"IsGameObjectNamed({expectedName})";
        }

        public override bool Evaluate(object activator = null)
        {
            if (activator is GameObject activatorObject)
            {
                if (!activatorObject)
                {
                    return false;
                }

                return activatorObject.name.Equals(expectedName, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}