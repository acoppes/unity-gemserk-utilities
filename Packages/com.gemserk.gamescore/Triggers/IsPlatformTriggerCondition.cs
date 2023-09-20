using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class IsPlatformTriggerCondition : TriggerCondition
    {
        public RuntimePlatform platform;
        
        public override string GetObjectName()
        {
            return $"IsPlatform({platform})";
        }

        public override bool Evaluate(object activator = null)
        {
            return Application.platform == platform;
        }
    }
}