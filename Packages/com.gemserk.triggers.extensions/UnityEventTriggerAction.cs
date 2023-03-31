using UnityEngine.Events;

namespace Gemserk.Triggers
{
    public class UnityEventTriggerAction : TriggerAction
    {
        public UnityEvent unityEvent;
        
        public override string GetObjectName()
        {
            if (unityEvent == null || unityEvent.GetPersistentEventCount() == 0)
            {
                return "UnityEvent()";
            }

            if (unityEvent.GetPersistentTarget(0) == null)
            {
                return "UnityEvent()";
            }

            // var persistentEvents = unityEvent.GetPersistentEventCount();
            
            // if (persistentEvents == 1)
            // {
            return $"UnityEvent({unityEvent.GetPersistentTarget(0).name}.{unityEvent.GetPersistentMethodName(0)})";
            // }
            // else
            // {
            //     return $"UnityEvent({unityEvent.GetPersistentTarget(0)}, {unityEvent.GetPersistentMethodName(0)})";
            // }
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            unityEvent.Invoke();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}