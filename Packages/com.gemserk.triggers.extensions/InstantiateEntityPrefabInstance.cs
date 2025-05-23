﻿using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class InstantiateEntityPrefabInstance : TriggerAction
    {
        [ObjectType(typeof(BaseEntityPrefabInstance), disableAssetReferences = true, disablePrefabReferences = true)]
        public Object entityInstance;

        public override string GetObjectName()
        {
            if (entityInstance)
            {
                return $"InstantiateEntity({entityInstance.name})";
            }
            return "InstantiateEntity()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
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
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}