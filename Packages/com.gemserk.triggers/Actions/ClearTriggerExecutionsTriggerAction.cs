using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers.Actions
{
    [TriggerEditor("Clear Trigger Executions", "Clears pending executions that could've been enqueued during execution")]
    public class ClearTriggerExecutionsTriggerAction : TriggerAction
    {
        public Object trigger;
        
        public override string GetObjectName()
        {
            if (trigger)
            {
                return $"ClearTriggerExecutions({trigger.name})";
            }
            return "ClearTriggerExecutions()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var t = trigger.GetInterface<ITrigger>();
            t.ClearPendingExecutions();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}