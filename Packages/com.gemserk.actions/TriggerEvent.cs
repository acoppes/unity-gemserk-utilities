namespace Gemserk.Actions
{
    public abstract class TriggerEvent : TriggerElement, ITrigger.IEvent
    {
        protected ITrigger trigger => GetComponentInParent<ITrigger>();
    }
}