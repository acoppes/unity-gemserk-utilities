using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Gemserk.Triggers
{
    public class CloneEntityTriggerAction : WorldTriggerAction
    {
        public enum CloneType
        {
            First,
            All
        }

        public CloneType cloneType;
        
        public TriggerTarget target;

        public override string GetObjectName()
        {
            return $"CloneEntity({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(target, activator);

            foreach (var entity in entities)
            {
                world.CreateEntity(entity);
                if (cloneType == CloneType.First)
                {
                    break;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}