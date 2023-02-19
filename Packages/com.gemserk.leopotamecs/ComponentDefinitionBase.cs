using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class ComponentDefinitionBase : MonoBehaviour, IComponentDefinition
    {
        public void Apply(World world, Entity entity)
        {
            
        }
    }
}