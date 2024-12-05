using System;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public class FirstTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        private TriggerAction action;
        
        public override string GetObjectName()
        {
            return $"First({target})";
        }

        private void Awake()
        {
            action = gameObject.GetComponentInChildrenDepth1<TriggerAction>(false, true);
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(target, activator);
            return action.Execute(entities[0]);
        }
    }
}