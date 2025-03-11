using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers.Actions
{
    [TriggerEditor("Invoke Trigger")]
    public class InvokeTriggerAction : TriggerAction
    {
        public Object trigger;

        public bool forceExecution;

        public bool dontInvokeIfDisabled;

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

            if (dontInvokeIfDisabled && t.IsDisabled())
            {
                return ITrigger.ExecutionResult.Completed;
            }
            
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