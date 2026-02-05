using UnityEngine;

namespace Gemserk.Triggers
{
    public abstract class TriggerAction : MonoBehaviour, ITrigger.IAction, ITriggerDebugNamedObject
    {
        public bool Disabled => !gameObject.activeSelf || !enabled;
        
        public abstract ITrigger.ExecutionResult Execute(object activator = null);

        public virtual string GetObjectName()
        {
            return string.Empty;
        }
    }
}