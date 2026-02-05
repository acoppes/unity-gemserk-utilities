using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public abstract class TriggerCondition : MonoBehaviour, ITrigger.ICondition, ITriggerDebugNamedObject
    {
        public bool Disabled => !gameObject.activeSelf || !enabled;
        public abstract bool Evaluate(object activator = null);
        
        public virtual string GetObjectName()
        {
            return string.Empty;
        }
    }
}