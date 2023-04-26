using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstance : MonoBehaviour
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
            Automatic = 1,
        }
        
        [FormerlySerializedAs("entityDefinitionPrefab")] 
        public UnityEngine.Object entityDefinition;

        [NonSerialized]
        public Entity instance = Entity.NullEntity;

        // rename to link type
        [FormerlySerializedAs("instanceType")] 
        public OnInstantiateActionType onInstantiateActionType = OnInstantiateActionType.Disable;

        // rename to instantiation type?
        [FormerlySerializedAs("autoInstantiateOnAwake")] 
        public InstantiationType instantiationType = InstantiationType.Automatic;

        // could be something like:
        // linkType: None, LinkWithGameObject 
        // instantiationType: OnAwake, OnStart, OnEnable, Manual
        // onInstantiateType: None, AutoDisable, AutoDestroy
        
        // cant AutoDestroy or AutoDisable with LinkWithGameObject

        private void OnEnable()
        {
            if (instantiationType == InstantiationType.Automatic)
            {
                InstantiateEntity();
            }
        }

        public void InstantiateEntity()
        {
            var world = World.Instance;

            if (world != null)
            {
                var instanceEntity = world.CreateEntity();
                world.AddComponent(instanceEntity, new EntityPrefabComponent()
                {
                    prefabInstance = this
                });
            }
        }
    }
}