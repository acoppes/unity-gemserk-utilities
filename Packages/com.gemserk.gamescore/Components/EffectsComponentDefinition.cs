using System;
using System.Collections.Generic;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    [Serializable]
    public struct Effect
    {
        public enum TargetType
        {
            Target = 0, 
            Source = 1,
            TargetsFromTargeting = 2
        }
        
        // public int type;
        public float value;
        
        public TargetType targetType;
        public Targeting targeting;
    }
    
    public struct EffectsComponent : IEntityComponent
    {
        public Target target;
        public Entity source;
        public List<Effect> effects;
        // public float factor;
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