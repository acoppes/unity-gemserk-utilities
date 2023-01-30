using Gemserk.Utilities;

namespace Gemserk.Actions
{
    public abstract class TriggerCondition : AutoNamedObject, ITrigger.ICondition
    {
        public abstract bool Evaluate(object activator = null);
    }
}