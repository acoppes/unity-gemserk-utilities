using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Triggers.Queries;
using MyBox;

namespace Gemserk.Triggers
{
    public class DestroyTriggerAction : WorldTriggerAction
    {
        public enum Version
        {
            V0,
            V1
        }

        public Version version = Version.V0;

        [ConditionalField(nameof(version), false, Version.V0)]
        public Query query;

        [ConditionalField(nameof(version), false, Version.V1)]
        public TriggerTarget target;

        public bool failIfNoTargets;
        
        public override string GetObjectName()
        {
            if (version == Version.V0 && query != null)
            {
                return $"Destroy({query})";
            }
            
            if (version == Version.V1)
            {
                return $"Destroy({target})";
            }

            return "Destroy()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            if (version == Version.V0)
            {
                entities = world.GetEntities(query.GetEntityQuery());
            } else if (version == Version.V1)
            {
                target.Get(entities, world, activator);
            }
            
            foreach (var entity in entities)
            {
                ref var destroyableComponent = ref world.GetComponent<DestroyableComponent>(entity);
                destroyableComponent.destroy = true;
            }

            if (failIfNoTargets && entities.Count == 0)
            {
                throw new Exception("No targets found to execute trigger.");
                // return ITrigger.ExecutionResult.Interrupt;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}