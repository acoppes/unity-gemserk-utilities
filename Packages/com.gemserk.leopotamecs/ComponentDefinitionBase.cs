using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class ComponentDefinitionBase : MonoBehaviour, IComponentDefinition, IEntityInstanceParameter
    {
        public abstract string GetComponentName();
        
        public abstract void Apply(World world, Entity entity);
    }
}