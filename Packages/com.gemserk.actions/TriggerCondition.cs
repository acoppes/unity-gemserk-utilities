namespace Gemserk.Actions
{
    public abstract class TriggerCondition : TriggerElement, ITrigger.ICondition
    {
        public abstract bool Evaluate(object activator = null);
    }
}