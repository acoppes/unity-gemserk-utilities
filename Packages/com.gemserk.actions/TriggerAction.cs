namespace Gemserk.Actions
{
    public abstract class TriggerAction : TriggerElement, ITrigger.IAction
    {
        public abstract ITrigger.ExecutionResult Execute(object activator = null);
    }
}