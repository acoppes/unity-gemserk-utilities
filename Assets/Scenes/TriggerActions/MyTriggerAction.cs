using Gemserk.Triggers;
using UnityEngine;

[TriggerEditor("MyAction()", tooltip = "This action has a tooltip example.")]
public class MyTriggerAction : TriggerAction
{
    public float value1;

    public override string GetObjectName()
    {
        return $"Debug({value1})";
    }

    public override ITrigger.ExecutionResult Execute(object activator = null)
    {
        Debug.Log($"VALUE: {value1}");
        return ITrigger.ExecutionResult.Completed;
    }
}