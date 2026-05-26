using System;
using System.Collections.Generic;
using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Gemserk.Leopotam.Ecs
{
    public static class BaseEntityPrefabUtils
    {
        public static void SpawnEntityPrefab(Object entityInstance)
        {
            if (!entityInstance)
            {
                return;
            }

            if (entityInstance is GameObject entityObject)
            {
                var prefabInstances = entityObject.GetComponentsInChildren<BaseEntityPrefabInstance>();
                foreach (var baseEntityPrefabInstance in prefabInstances)
                {
                    baseEntityPrefabInstance.InstantiateEntity();
                }
                return;
            }
                
            var entityPrefabInstance = entityInstance.GetInterface<BaseEntityPrefabInstance>();
            if (entityPrefabInstance)
            {
                entityPrefabInstance.InstantiateEntity();
            }
        }
    }
    
    public abstract class BaseEntityPrefabInstance : MonoBehaviour
    {
        public enum OnInstantiateActionType
        {
            // InstantiateAndDestroy = 0,
            Disable = 0,
            LinkObject = 2,
            Destroy = 3, 
            None = 4
        }
        
        public enum InstantiationType
        {
            Manual = 0,
            AutomaticOnEnable = 1,
            AutomaticOnceOnStart = 2
        }
        
        // rename to link type
        [FormerlySerializedAs("instanceType")] 
        public OnInstantiateActionType onInstantiateActionType = OnInstantiateActionType.Disable;
        
        // rename to instantiation type?
        [FormerlySerializedAs("autoInstantiateOnAwake")] 
        public InstantiationType instantiationType = InstantiationType.AutomaticOnEnable;
        
        [NonSerialized]
        public Entity instance = Entity.NullEntity;

        // could be something like:
        // linkType: None, LinkWithGameObject 
        // instantiationType: OnAwake, OnStart, OnEnable, Manual
        // onInstantiateType: None, AutoDisable, AutoDestroy
        
        // cant AutoDestroy or AutoDisable with LinkWithGameObject

        public WorldReference worldReference;

        public abstract IEntityDefinition GetEntityDefinition();
        
        private void OnEnable()
        {
            if (instantiationType == InstantiationType.AutomaticOnEnable)
            {
                InstantiateEntity();
            }
        }

        private void Start()
        {
            if (instantiationType == InstantiationType.AutomaticOnceOnStart)
            {
                InstantiateEntity();
            }
        }

        public void InstantiateEntity()
        {
            // var world = GameObject.FindWithTag(worldTag).GetComponent<World>();
            var world = worldReference.GetReference(gameObject);

            var instanceEntity = world.CreateEntity();
            world.AddComponent(instanceEntity, new EntityPrefabComponent()
            {
                prefabInstance = this
            });
        }

        public void GetEntityParameters(List<IEntityInstanceParameter> list)
        {
            gameObject.GetComponents<IEntityInstanceParameter>(list);
        }

    }
}