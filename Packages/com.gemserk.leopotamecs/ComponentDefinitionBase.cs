using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    [Serializable]
    public abstract class ComponentDefinitionBase : IComponentDefinition
    {
        // This is just a hack to show it with proper name in insspector.
        [HideInInspector]
        public string name;

        [NonSerialized]
        public GameObject gameObject;

        public ComponentDefinitionBase()
        {
            name = GetType().Name.Replace("Definition", "");
        }
        
        public virtual void Apply(World world, Entity entity)
        {
            
        }
    }
}