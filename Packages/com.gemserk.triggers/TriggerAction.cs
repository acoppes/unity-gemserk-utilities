using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public abstract class TriggerAction : AutoNamedObject, ITrigger.IAction
    {
        public abstract ITrigger.ExecutionResult Execute(object activator = null);
    }
}