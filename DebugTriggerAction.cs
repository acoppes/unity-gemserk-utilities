using UnityEngine;

namespace Gemserk.Triggers
{
    public class DebugTriggerAction : TriggerAction
    {
        public string text;
        
        public override string GetObjectName()
        {
            return "DebugLog()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            Debug.Log(text);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}