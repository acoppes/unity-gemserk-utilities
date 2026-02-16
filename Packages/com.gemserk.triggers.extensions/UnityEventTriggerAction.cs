using UnityEngine;
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

            // return "UnityEvent";

            // var persistentEvents = unityEvent.GetPersistentEventCount();
            
            // if (persistentEvents == 1)
            // {
            var persistentTarget = unityEvent.GetPersistentTarget(0);
            var method = unityEvent.GetPersistentMethodName(0);

            if (persistentTarget is Component m)
            {
                if (m.gameObject == gameObject)
                {
                    return $"UnityEvent({m.GetType().Name}.{method})";
                }
            }
            
            return $"UnityEvent({persistentTarget.name}.{method})";
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