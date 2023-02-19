using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ObjectEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        [SerializeReference]
        public List<IComponentDefinition> componentDefinitions = new List<IComponentDefinition>();

        public bool hideMonoBehaviours = true;

        public virtual void Apply(World world, Entity entity)
        {
            foreach (var componentDefinition in componentDefinitions)
            {
                var definitionBase = componentDefinition as ComponentDefinitionSerializedObjectBase;
                
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

            var componentDefinitionsFromObjects = GetComponentsInChildren<IComponentDefinition>();
            
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