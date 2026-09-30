using Gemserk.Triggers;
using Gemserk.Utilities;
using UnityEngine;

public class InvokeTriggerWithActivatorAction : TriggerAction
{
    public Object trigger;

    public bool forceExecution;

    public bool dontInvokeIfDisabled;

    public Object customActivator;

    public override string GetObjectName()
    {
        if (trigger)
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
            t.QueueExecution(customActivator);
        }
        else
        {
            t.ForceQueueExecution(customActivator);
        }
            
        return ITrigger.ExecutionResult.Completed;
    }
}