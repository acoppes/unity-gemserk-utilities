using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ObjectEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        public virtual void Apply(World world, Entity entity)
        {
            var componentDefinitionsFromObjects = GetComponents<IComponentDefinition>();
            
            foreach (var componentDefinitionObject in componentDefinitionsFromObjects)
            {
                if (componentDefinitionObject != null)
                {
                    componentDefinitionObject.Apply(world, entity);
                }
            }
        }
    }
}