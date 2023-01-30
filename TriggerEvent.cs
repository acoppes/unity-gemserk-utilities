using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public abstract class TriggerEvent : AutoNamedObject, ITrigger.IEvent
    {
        protected ITrigger trigger => GetComponentInParent<ITrigger>();
    }
}