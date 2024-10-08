using System;
using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gemserk.Triggers
{
    [Obsolete("Use new wait trigger")]
    public class OldWaitTriggerAction : TriggerAction
    {
        [FormerlySerializedAs("cooldown")] 
        public Cooldown time = new Cooldown(0);
        
        public override string GetObjectName()
        {
            return $"Wait({time.Total})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            time.Increase(Time.deltaTime);
            
            if (!time.IsReady)
            {
                return ITrigger.ExecutionResult.Running;
            }
            
            time.Reset();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}