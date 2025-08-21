using System.Collections.Generic;
using System.Linq;
using Game.Utilities;
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
        public float effectValueMultiplier;
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
                effectValueMultiplier = 1f,
                effectDefinitions = effectDefinitions.Select(e => e.GetInterface<IEntityDefinition>()).ToList()
            });
        }
    }
}