﻿using System;
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
            Source = 1
        }
        
        public EffectType type;

        public float minValue;
        public float maxValue;

        public ValueCalculationType valueCalculationType;
        
        public TargetType targetType;
    }
    
    public struct EffectsComponent : IEntityComponent
    {
        public Target target;
        public Entity source;
        public List<Effect> effects;
        // public float factor;

        public float factor;
        
        public int minDelay;
        public int maxDelay;

        public int delayFramesToApply;
        public bool hasDelaySet;

        public int currentFrame;
    }
    
    public class EffectsComponentDefinition : ComponentDefinitionBase
    {
        public List<Effect> effects;
        
        public int minDelay;
        public int maxDelay;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new EffectsComponent()
            {
                effects = effects,
                factor = 1,
                minDelay = minDelay,
                maxDelay = maxDelay
            });
        }
    }
}