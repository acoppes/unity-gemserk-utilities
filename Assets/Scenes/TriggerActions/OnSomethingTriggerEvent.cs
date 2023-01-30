using Gemserk.Triggers;
using UnityEngine;

public class OnSomethingTriggerEvent : TriggerEvent
{
    [ContextMenu("Trigger Execution")]
    public void TriggerExecution()
    {
        trigger.QueueExecution();
    }
}