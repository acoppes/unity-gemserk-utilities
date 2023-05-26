using System.Collections.Generic;
using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public class AndTriggerCondition : TriggerCondition
    {
        private readonly List<TriggerCondition> conditions = new();

        private void Awake()
        {
            gameObject.GetComponentsInChildrenDepth1(false, true, conditions);
        }

        public override string GetObjectName()
        {
            return "And()";
        }

        public override bool Evaluate(object activator = null)
        {
            foreach (var condition in conditions)
            {
                if (!condition.Evaluate(activator))
                {
                    return false;
                }
            }
            return true;
        }
    }
}