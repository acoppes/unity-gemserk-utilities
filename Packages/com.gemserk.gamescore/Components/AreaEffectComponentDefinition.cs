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
        public float rangeMultiplier;
        public Vector3 direction;
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
                rangeMultiplier = 1f,
                direction = new Vector3(1, 0, 0),
                effectDefinitions = effectDefinitions.Select(e => e.GetInterface<IEntityDefinition>()).ToList()
            });
        }
    }
}