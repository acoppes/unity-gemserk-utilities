using Gemserk.Triggers;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Actions
{
    [TriggerEditor("Invoke Trigger")]
    public class InvokeTriggerAction : TriggerAction
    {
        public Object trigger;

        public bool forceExecution;

        public override string GetObjectName()
        {
            if (trigger != null)
            {
                return $"Invoke({trigger.name}, force:{forceExecution})";
            }
            return "Invoke()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var t = trigger.GetInterface<ITrigger>();
            
            if (!forceExecution)
            {
                t.QueueExecution(activator);
            }
            else
            {
                t.ForceQueueExecution(activator);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}