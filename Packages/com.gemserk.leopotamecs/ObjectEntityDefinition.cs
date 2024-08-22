using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ObjectEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        public void Apply(World world, Entity entity)
        {
            var componentDefinitionsFromObjects = GetComponents<IComponentDefinition>();
            
            foreach (var componentDefinitionObject in componentDefinitionsFromObjects)
            {
                if (componentDefinitionObject is MonoBehaviour m)
                {
                    if (!m.enabled)
                    {
                        continue;
                    }
                }
                
                componentDefinitionObject?.Apply(world, entity);
            }
        }
    }
}