using System;
using System.Collections.Generic;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    [Serializable]
    public struct Effect
    {
        public enum EffectType
        {
            Damage = 0
        }

        public enum ValueCalculationType
        {
            Random = 0,
            BasedOnFactor = 1
        }
        
        public enum TargetType
        {
            Target = 0, 
            Source = 1,
            
            // deprecated
            TargetsFromTargeting = 2
        }
        
        public EffectType type;

        public float minValue;
        public float maxValue;

        public ValueCalculationType valueCalculationType;
        
        public TargetType targetType;
        public Targeting targeting;
    }
    
    public struct EffectsComponent : IEntityComponent
    {
        public Target target;
        public Entity source;
        public List<Effect> effects;
        // public float factor;

        public float factor;
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
                effects = effects,
                factor = 1
            });
        }
    }
}