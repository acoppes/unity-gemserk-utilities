using System.Collections.Generic;
using Gemserk.Utilities;

namespace Gemserk.Triggers.Conditions
{
    public class OrTriggerCondition : TriggerCondition
    {
        private readonly List<TriggerCondition> conditions = new();

        private void Awake()
        {
            gameObject.GetComponentsInChildrenDepth1(false, true, conditions);
        }

        public override string GetObjectName()
        {
            return "Or()";
        }

        public override bool Evaluate(object activator = null)
        {
            foreach (var condition in conditions)
            {
                if (condition.Evaluate(activator))
                {
                    return true;
                }
            }

            return false;
        }
    }
}