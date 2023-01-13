using UnityEngine;

namespace Gemserk.Actions
{
    public abstract class TriggerEvent : MonoBehaviour, ITrigger.IEvent
    {
        protected ITrigger trigger => GetComponentInParent<ITrigger>();
    }
}