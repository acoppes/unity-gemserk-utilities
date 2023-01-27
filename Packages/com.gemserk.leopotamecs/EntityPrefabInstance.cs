using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstance : MonoBehaviour
    {
        public enum InstanceType
        {
            // InstantiateAndDestroy = 0,
            InstantiateAndDisable = 0,
            InstantiateAndLink = 2,
            InstantiateAndDestroy = 3
        }
        
        [FormerlySerializedAs("entityDefinitionPrefab")] 
        public UnityEngine.Object entityDefinition;

        [NonSerialized]
        public Entity instance = Entity.NullEntity;

        public InstanceType instanceType = InstanceType.InstantiateAndDisable;

        private void Awake()
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