using System;
using Gemserk.Utilities;

namespace Gemserk.Triggers.Conditions
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
            if (!condition)
            {
                throw new Exception("Can't execute Not() without inner conditions to check.");
            }
            
            return !condition.Evaluate(activator);
        }
    }
}