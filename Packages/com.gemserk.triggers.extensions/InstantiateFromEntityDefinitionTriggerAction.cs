using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class InstantiateFromEntityDefinitionTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
            return $"InstantiateFromDefinition({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = world.GetEntities(target, activator);

            foreach (var entityTarget in targets)
            {
                var definitionComponent = entityTarget.Get<EntityDefinitionComponent>();
                world.CreateEntity(definitionComponent.definition, definitionComponent.parameters);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}