using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public abstract class TriggerAction : AutoNamedObject, ITrigger.IAction
    {
        public bool Disabled => !gameObject.activeSelf || !enabled;
        
        public abstract ITrigger.ExecutionResult Execute(object activator = null);
    }
}