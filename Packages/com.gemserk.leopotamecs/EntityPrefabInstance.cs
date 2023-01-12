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
            InstantiateAndLink = 2 
        }
        
        [FormerlySerializedAs("entityDefinitionPrefab")] 
        public UnityEngine.Object entityDefinition;

        [NonSerialized]
        public Entity instance = Entity.NullEntity;

        public InstanceType instanceType = InstanceType.InstantiateAndDisable;
    }
}