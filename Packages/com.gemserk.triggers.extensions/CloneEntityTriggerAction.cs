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

        private Type[] componentTypesCache = new Type[100];

        public override string GetObjectName()
        {
            return $"CloneEntity({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(target, activator);

            foreach (var entity in entities)
            {
                var clonedEntity = world.CreateEntity(entity);
                
                var components = world.EcsWorld.GetComponentTypes(entity.ecsEntity, ref componentTypesCache);

                for (var i = 0; i < components; i++)
                {
                    var componentType = componentTypesCache[i];
                    var component = world.GetComponent(entity, componentType);
                    
                    // if (!world.EcsWorld.GetPoolByType(componentType).Has(clonedEntity.ecsEntity))
                    // {
                    //     world.EcsWorld.GetPoolByType(componentType).AddRaw(clonedEntity.ecsEntity, component);
                    // }
                    // else
                    // {
                    //     world.EcsWorld.GetPoolByType(componentType).SetRaw(clonedEntity.ecsEntity, component);
                    // }
                    
                    // world.AddComponent(clonedEntity.ecsEntity, component);
                }

                if (cloneType == CloneType.First)
                {
                    break;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}