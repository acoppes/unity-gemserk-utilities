using System;
using System.Collections.Generic;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    [Serializable]
    public struct Effect
    {
        // public int type;
        public float value;
    }
    
    public struct EffectsComponent : IEntityComponent
    {
        public Target target;
        public Entity source;
        public List<Effect> effects;
    }
    
    public class EffectsComponentDefinition : ComponentDefinitionBase
    {
        public List<Effect> effects;
        
        public override string GetComponentName()
        {
            return nameof(EffectsComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new EffectsComponent()
            {
                effects = effects
            });
        }
    }
}