using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Components
{
    public class PackComponentInstance : MonoBehaviour, IEntityInstanceParameter
    {
        public List<Object> definitions;
        
        public void Apply(World world, Entity entity)
        {
            ref var pack = ref entity.Get<PackComponent>();
            pack.definitions = definitions.Select(o => o.GetInterface<IEntityDefinition>()).ToList();
        }
    }
}