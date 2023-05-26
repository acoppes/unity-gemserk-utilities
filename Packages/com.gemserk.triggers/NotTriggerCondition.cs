using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public class NotTriggerCondition : TriggerCondition
    {
        private TriggerCondition condition;

        private void Awake()
        {
            condition = gameObject.GetComponentInChildrenDepth1<TriggerCondition>(false, true);
        }

        public override string GetObjectName()
        {
            return "Not()";
        }

        public override bool Evaluate(object activator = null)
        {
            return !condition.Evaluate(activator);
        }
    }
}