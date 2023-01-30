using Gemserk.Utilities;

namespace Gemserk.Actions
{
    public abstract class TriggerEvent : AutoNamedObject, ITrigger.IEvent
    {
        protected ITrigger trigger => GetComponentInParent<ITrigger>();
    }
}