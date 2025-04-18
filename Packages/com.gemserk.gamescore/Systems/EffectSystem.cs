﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Systems
{
    public class EffectSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EffectsComponent, PositionComponent, PlayerComponent>, Exc<DisabledComponent>> effectsFilter = default;
        readonly EcsFilterInject<Inc<EffectsComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyableEffectsFilter = default;
        
        readonly EcsFilterInject<Inc<AreaEffectComponent, PositionComponent, PlayerComponent>, Exc<DisabledComponent>> areaEffects = default;
        readonly EcsFilterInject<Inc<AreaEffectComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyableAreaEffects = default;
        
        private static readonly List<Target> targets = new List<Target>();
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in effectsFilter.Value)
            {
                // var cursor = ref cursorInputFilter.Pools.Inc1.Get(e);
                ref var effects = ref effectsFilter.Pools.Inc1.Get(e);
                var position = effectsFilter.Pools.Inc2.Get(e);
                var player = effectsFilter.Pools.Inc3.Get(e);

                if (!effects.hasDelaySet && effects.maxDelay > 0 && effects.minDelay >= 0)
                {
                    effects.delayFramesToApply = UnityEngine.Random.Range(effects.minDelay, effects.maxDelay);
                    effects.hasDelaySet = true;
                }

                effects.currentFrame++;

                if (effects.currentFrame < effects.delayFramesToApply)
                {
                    continue;
                }
                
                foreach (var effect in effects.effects)
                {
                    // var modifiedEffect = effect;
                    // modifiedEffect.value = effect.value * effects.factor;
                    
                    if (effect.targetType == Effect.TargetType.Target)
                    {
                        if (effects.target != null && effects.target.entity.Exists())
                        {
                            // ApplyDamageEffect(effects.target.entity, effects.source, modifiedEffect, position.value);
                            ApplyEffect(effects.factor, effects.target, effects.source, effect, position.value, player.player);
                        }
                    }

                    if (effect.targetType == Effect.TargetType.Source)
                    {
                        if (effects.source.Exists())
                        {
                            // ApplyDamageEffect(effects.source, effects.source, modifiedEffect, position.value);
                            ApplyEffect(effects.factor, effects.source.Get<TargetComponent>().target, effects.source, effect, position.value, player.player);
                        }
                    }

                    // if (effect.targetType == Effect.TargetType.TargetsFromTargeting)
                    // {
                    //     if (position.type == 0)
                    //     {
                    //         var normal = Quaternion.Euler(-45, 0, 0) * Vector3.up;
                    //
                    //         D.raw(new Shape.Circle(GamePerspective.ConvertFromWorld(position.value), normal,
                    //             Vector3.right, effect.targeting.targetingFilter.range.Max), Color.red, 1f);
                    //     }
                    //     else if (position.type == 1)
                    //     {
                    //         D.raw(new Shape.Circle(position.value, Quaternion.identity, 
                    //             effect.targeting.targetingFilter.range.Max), Color.red, 1f);
                    //     }
                    //     
                    //     world.GetTargets(new RuntimeTargetingParameters()
                    //     {
                    //         alliedPlayersBitmask = player.GetAlliedPlayers(),
                    //         position = position.value,
                    //         direction = new Vector3(1, 0, 0),
                    //         filter = effect.targeting.targetingFilter
                    //     }, targets);
                    //
                    //     foreach (var target in targets)
                    //     {
                    //         ApplyEffect(effect.factor, target, effects.source, effect, position.value);
                    //         
                    //         // if(effect.type == Effect.EffectType.Damage)
                    //         // {
                    //         //     ApplyDamageEffect(target.entity, effects.source, modifiedEffect, position.value);
                    //         // } else if(effect.type == Effect.EffectType.AreaDamage)
                    //         // {
                    //         //     ApplyAreaDamageEffect(modifiedEffect.targeting.targetingFilter.maxRangeSqr, target, effects.source, modifiedEffect, position.value);
                    //         // }
                    //     }
                    //     
                    //     targets.Clear();
                    // }

                }
            }
            
            foreach (var e in destroyableEffectsFilter.Value)
            {
                var effects = effectsFilter.Pools.Inc1.Get(e);
                ref var destroyable = ref destroyableEffectsFilter.Pools.Inc2.Get(e);
                
                if (effects.currentFrame >= effects.delayFramesToApply)
                {
                    destroyable.destroy = true;
                }
            }
            
            // COPY DIRECTION FROM LOOKING DIRECTION?
            
            foreach (var e in areaEffects.Value)
            {
                // var cursor = ref cursorInputFilter.Pools.Inc1.Get(e);
                ref var areaEffect = ref areaEffects.Pools.Inc1.Get(e);
                var position = areaEffects.Pools.Inc2.Get(e);
                var player = areaEffects.Pools.Inc3.Get(e);
                
                world.GetTargets(new RuntimeTargetingParameters()
                {
                    alliedPlayersBitmask = player.GetAlliedPlayers(),
                    position = position.value,
                    direction = new Vector3(1, 0, 0),
                    filter = areaEffect.targeting.targetingFilter
                }, targets);

                var rangeSqr = areaEffect.targeting.targetingFilter.maxRangeSqr;

                foreach (var target in targets)
                {
                    var distSqr = (target.position - position.value).sqrMagnitude;
                    
                    foreach (var effectDefinition in areaEffect.effectDefinitions)
                    {
                        var effectEntity = world.CreateEntity(effectDefinition);
                        // create effects
                        ref var effects = ref effectEntity.Get<EffectsComponent>();
                        effects.target = target;
                        effects.source = areaEffect.source;
                        effects.factor = Mathf.Clamp01(distSqr / rangeSqr);
                        
                        // damage = Mathf.Lerp(effect.maxValue, effect.minValue, factor);
                        
                        effectEntity.Get<PositionComponent>().value = position.value;
                        effectEntity.Get<PlayerComponent>().player = player.player;
                    }
                }
                        
                targets.Clear();
            }
            
            // TODO: AREA EFFECTS CAN DO EFFECTS OVER TIME, ONCE PER TARGET OR MULTIPLE TIMES, ETC (AND/OR RETARGET)
            foreach (var e in destroyableAreaEffects.Value)
            {
                ref var destroyable = ref destroyableAreaEffects.Pools.Inc2.Get(e);
                destroyable.destroy = true;
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static void ApplyDamageEffect(Entity target, Entity source, Effect effect, Vector3 position)
        // {
        //     ref var health = ref target.Get<HealthComponent>();
        //     health.damages.Add(new DamageData()
        //     {
        //         value = Random.Range(effect.minValue, effect.maxValue),
        //         position = position,
        //         knockback = false,
        //         source = source
        //     });
        // }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ApplyEffect(float factor, Target target, Entity source, Effect effect, Vector3 position, int player)
        {
            var value = 0f;
            
            if (effect.valueCalculationType == Effect.ValueCalculationType.BasedOnFactor && factor > 0)
            {
                value = Mathf.Lerp(effect.maxValue, effect.minValue, factor);
            }
            else if (effect.valueCalculationType == Effect.ValueCalculationType.Random)
            {
                value = Random.Range(effect.minValue, effect.maxValue);
            }
            else
            {
                value = effect.maxValue;
            }
            
            if (effect.type == Effect.EffectType.Damage && target.entity.Has<HealthComponent>())
            {
                ref var health = ref target.entity.Get<HealthComponent>();
                health.damages.Add(new DamageData
                {
                    value = value,
                    position = position,
                    knockback = false,
                    source = source,
                    player = player
                });
            }
        }
    }
}