using UnityEngine.Events;

namespace Gemserk.Triggers
{
    public class UnityEventTriggerAction : TriggerAction
    {
        public UnityEvent unityEvent;
        
        public override string GetObjectName()
        {
            if (unityEvent == null)
            {
                return "UnityEvent()";
            }
            return $"UnityEvent({unityEvent})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            unityEvent.Invoke();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}