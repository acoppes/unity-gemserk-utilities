using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public abstract class TriggerEvent : MonoBehaviour, ITrigger.IEvent, ITriggerDebugNamedObject
    {
        protected ITrigger trigger => GetComponentInParent<ITrigger>();
        
        public virtual string GetObjectName()
        {
            return string.Empty;
        }
    }
}