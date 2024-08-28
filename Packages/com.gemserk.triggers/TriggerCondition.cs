using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public abstract class TriggerCondition : AutoNamedObject, ITrigger.ICondition
    {
        public bool Disabled => !gameObject.activeSelf || !enabled;
        public abstract bool Evaluate(object activator = null);
    }
}