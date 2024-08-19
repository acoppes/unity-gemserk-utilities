using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Components
{
    public struct SpawnOnSpawnComponent : IEntityComponent
    {
        public List<IEntityDefinition> definitions;
    }
    
    public class SpawnOnSpawnComponentDefinition : ComponentDefinitionBase
    {
        public List<Object> definitions;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new SpawnOnSpawnComponent()
            {
                definitions = definitions.Select(d => d.GetInterface<IEntityDefinition>()).ToList()
            });
        }
    }
}