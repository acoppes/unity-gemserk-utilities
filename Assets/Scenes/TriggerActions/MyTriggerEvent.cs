using Gemserk.Actions;
using UnityEngine;

public class MyTriggerEvent : TriggerEvent
{
    [ContextMenu("Trigger Execution")]
    public void TriggerExecution()
    {
        trigger.QueueExecution();
    }
}