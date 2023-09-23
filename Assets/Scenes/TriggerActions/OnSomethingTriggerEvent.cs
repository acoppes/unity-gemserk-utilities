using Gemserk.Triggers;
using UnityEngine;

public class OnSomethingTriggerEvent : TriggerEvent
{
    public Object optionalActivator;
    
    [ContextMenu("Trigger Execution")]
    public void TriggerExecution()
    {
        trigger.QueueExecution(optionalActivator);
    }
}