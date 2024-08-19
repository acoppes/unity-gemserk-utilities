using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Components
{
    public struct AreaEffectComponent : IEntityComponent
    {
        public Entity source;
        public Targeting targeting;
        public List<IEntityDefinition> effectDefinitions;
    }
    
    public class AreaEffectComponentDefinition : ComponentDefinitionBase
    {
        public Targeting targeting;
        
        [ObjectType(typeof(IEntityDefinition), disableAssetReferences = true, filterString = "Definition")]
        public List<Object> effectDefinitions;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new AreaEffectComponent()
            {
                targeting = targeting,
                effectDefinitions = effectDefinitions.Select(e => e.GetInterface<IEntityDefinition>()).ToList()
            });
        }
    }
}