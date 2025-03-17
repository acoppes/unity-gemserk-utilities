using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers
{
    public class EnableEntityTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Enable = 0,
            Disable = 1
        }

        public ActionType actionType;
        public TriggerTarget target;

        public override string GetObjectName()
        {
            return $"{actionType}({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            target.Get(entities, world, activator);

            if (actionType == ActionType.Disable)
            {
                foreach (var entity in entities)
                {
                    entity.Add(new DisabledComponent());
                }                    
            }
            
            if (actionType == ActionType.Enable)
            {
                foreach (var entity in entities)
                {
                    entity.Add(new EnableDisabledComponent());
                }                    
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}