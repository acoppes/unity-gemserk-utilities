using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ObjectEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        [SerializeReference]
        public List<IComponentDefinition> componentDefinitions = new List<IComponentDefinition>();

        public virtual void Apply(World world, Entity entity)
        {
            foreach (var componentDefinition in componentDefinitions)
            {
                var definitionBase = componentDefinition as ComponentDefinitionBase;
                
                if (definitionBase != null)
                {
                    definitionBase.gameObject = gameObject;
                }
                
                componentDefinition.Apply(world, entity);
                
                if (definitionBase != null)
                {
                    definitionBase.gameObject = null;
                }
            }
        }
    }
}