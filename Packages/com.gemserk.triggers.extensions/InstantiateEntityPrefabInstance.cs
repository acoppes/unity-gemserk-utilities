using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class InstantiateEntityPrefabInstance : WorldTriggerAction
    {
        public enum SourceType
        {
            FromEntityPrefab = 0,
            FromDefinition = 1
        }

        public SourceType sourceType = SourceType.FromEntityPrefab;
        
        [ConditionalField(nameof(sourceType), false, SourceType.FromEntityPrefab)]
        [ObjectType(typeof(BaseEntityPrefabInstance), disableAssetReferences = true, disablePrefabReferences = true)]
        public Object entityInstance;

        [ConditionalField(nameof(sourceType), false, SourceType.FromDefinition)]
        [ObjectType(typeof(IEntityDefinition), disableAssetReferences = true, disablePrefabReferences = false)]
        public Object entityDefinition;
        
        public override string GetObjectName()
        {
            if (sourceType == SourceType.FromEntityPrefab)
            {
                if (entityInstance)
                {
                    return $"InstantiateEntity{sourceType}({entityInstance.name})";
                }
            } else if (sourceType == SourceType.FromDefinition)
            {
                if (entityDefinition)
                {
                    return $"InstantiateEntity{sourceType}({entityDefinition.name})";
                }
            }
            return "InstantiateEntity()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (sourceType == SourceType.FromEntityPrefab)
            {
                if (!entityInstance)
                {
                    return ITrigger.ExecutionResult.Completed;
                }

                var entityPrefabInstance = entityInstance.GetInterface<BaseEntityPrefabInstance>();
            
                if (!entityPrefabInstance)
                {
                    return ITrigger.ExecutionResult.Completed;
                }
            
                entityPrefabInstance.InstantiateEntity();
            } else if (sourceType == SourceType.FromDefinition)
            {
                if (!entityDefinition)
                {
                    return ITrigger.ExecutionResult.Completed;
                }

                world.CreateEntity(entityDefinition);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}