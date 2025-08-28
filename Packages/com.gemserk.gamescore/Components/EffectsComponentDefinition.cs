using System;
using System.Collections.Generic;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Components
{
    public interface ICustomEffect
    {
        void ApplyEffect(float calculatedValue, EffectsComponent effects, Target target, Entity source, Effect effect);
    }
    
    [Serializable]
    public struct Effect
    {
        public enum EffectType
        {
            Damage = 0,
            Custom = 1
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
        
        [ObjectType(typeof(ICustomEffect), assetReferencesOnWhenStart = false, disableAssetReferences = false, 
            disablePrefabReferences = false, prefabReferencesOnWhenStart = true, 
            disableSceneReferences = true, filterString = "Effect")]
        public UnityEngine.Object customEffect;
    }
    
    public struct EffectsComponent : IEntityComponent
    {
        public Target target;
        public Entity source;
        public List<Effect> effects;
        // public float factor;

        public float factor;
        public float valueMultiplier;
        
        public int minDelay;
        public int maxDelay;

        public int delayFramesToApply;
        public bool hasDelaySet;

        public int currentFrame;

        public Vector3 position;
        public Vector3 direction;
        public int player;
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
                factor = 1f,
                valueMultiplier = 1f,
                minDelay = minDelay,
                maxDelay = maxDelay
            });
        }
    }
}