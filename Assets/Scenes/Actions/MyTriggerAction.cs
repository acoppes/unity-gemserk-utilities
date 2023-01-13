using Gemserk.Actions;
using UnityEngine;

public class MyTriggerAction : MonoBehaviour, ITrigger.IAction
{
    public float value1;
    
    public ITrigger.ExecutionResult Execute()
    {
        Debug.Log($"VALUE: {value1}");
        return ITrigger.ExecutionResult.Completed;
    }
}