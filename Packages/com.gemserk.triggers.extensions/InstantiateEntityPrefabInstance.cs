using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class InstantiateEntityPrefabInstance : TriggerAction
    {
        public Object entityInstance;
        
        public bool overridePosition;

        [ConditionalField(nameof(overridePosition))]
        public Transform position;
        
        public override string GetObjectName()
        {
            if (entityInstance != null)
            {
                return $"InstantiateEntity({entityInstance.name})";
            }
            return "InstantiateEntity()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (entityInstance == null)
            {
                return ITrigger.ExecutionResult.Completed;
            }

            if (overridePosition && entityInstance is GameObject entityInstanceObject)
            {
                entityInstanceObject.transform.position = position.position;
            }

            var entityPrefabInstance = entityInstance.GetInterface<EntityPrefabInstance>();
            
            if (entityPrefabInstance == null)
            {
                return ITrigger.ExecutionResult.Completed;
            }
            
            entityPrefabInstance.InstantiateEntity();
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}