using System;
using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    [Obsolete("Use ObjectEntityDefinition")]
    public class PrefabEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        public void Apply(World world, Entity entity)
        {
            var subDefinitions = GetComponentsInChildren<IEntityDefinition>().ToList();
            subDefinitions.Remove(this);
            foreach (var definition in subDefinitions)
            {
                definition.Apply(world, entity);
            }
        }
    }
}