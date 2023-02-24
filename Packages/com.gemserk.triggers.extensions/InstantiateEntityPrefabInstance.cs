using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class InstantiateEntityPrefabInstance : TriggerAction
    {
        public Object entityInstance;

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