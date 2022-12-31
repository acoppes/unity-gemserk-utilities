using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    [Serializable]
    public abstract class EntityComponentDefinitionBase : IEntityComponentDefinition
    {
        [HideInInspector]
        public string name;

        [NonSerialized]
        public GameObject gameObject;

        public EntityComponentDefinitionBase()
        {
            name = GetType().Name.Replace("Definition", "");
        }
        
        public virtual void Apply(World world, Entity entity)
        {
            
        }
    }
}