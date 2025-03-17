using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Gemserk.Triggers
{
    [Obsolete("Old version of this action, use new EnableEntity instead.")]
    public class EnableEntityTriggerActionV1 : WorldTriggerAction
    {
        public enum ActionType
        {
            Enable = 0,
            Disable = 1
        }

        public Query query;
        
        public ActionType actionType;

        public override string GetObjectName()
        {
            if (query == null)
            {
                return $"{actionType}()";
            }
            
            return $"{actionType}({query})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query);

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