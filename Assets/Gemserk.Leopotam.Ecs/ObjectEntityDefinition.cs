using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ObjectEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        [SerializeReference]
        public List<IEntityComponentDefinition> componentDefinitions = new List<IEntityComponentDefinition>();

        public void Apply(World world, Entity entity)
        {
            foreach (var componentDefinition in componentDefinitions)
            {
                componentDefinition.Apply(world, entity);
            }
        }
    }
}