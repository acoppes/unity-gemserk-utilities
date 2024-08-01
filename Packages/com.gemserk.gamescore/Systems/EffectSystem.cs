using System.Collections.Generic;
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

        private static readonly List<Target> targets = new List<Target>();
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in effectsFilter.Value)
            {
                // var cursor = ref cursorInputFilter.Pools.Inc1.Get(e);
                ref var effects = ref effectsFilter.Pools.Inc1.Get(e);
                var position = effectsFilter.Pools.Inc2.Get(e);
                var player = effectsFilter.Pools.Inc3.Get(e);
                
                foreach (var effect in effects.effects)
                {
                    var modifiedEffect = effect;
                    // modifiedEffect.value = effect.value * effects.factor;
                    
                    if (modifiedEffect.targetType == Effect.TargetType.Target)
                    {
                        if (effects.target != null && effects.target.entity.Exists())
                        {
                            ApplyDamageEffect(effects.target.entity, effects.source, modifiedEffect, position.value);
                        }
                    }

                    if (modifiedEffect.targetType == Effect.TargetType.Source)
                    {
                        if (effects.source.Exists())
                        {
                            ApplyDamageEffect(effects.source, effects.source, modifiedEffect, position.value);
                        }
                    }

                    if (modifiedEffect.targetType == Effect.TargetType.TargetsFromTargeting)
                    {
                        if (position.type == 0)
                        {
                            var normal = Quaternion.Euler(-45, 0, 0) * Vector3.up;

                            D.raw(new Shape.Circle(GamePerspective.ConvertFromWorld(position.value), normal,
                                Vector3.right, modifiedEffect.targeting.targetingFilter.range.Max), Color.red, 1f);
                        }
                        else if (position.type == 1)
                        {
                            D.raw(new Shape.Circle(position.value, Quaternion.identity, 
                                modifiedEffect.targeting.targetingFilter.range.Max), Color.red, 1f);
                        }
                        
                        world.GetTargets(new RuntimeTargetingParameters()
                        {
                            alliedPlayersBitmask = player.GetAlliedPlayers(),
                            position = position.value,
                            direction = new Vector3(1, 0, 0),
                            filter = modifiedEffect.targeting.targetingFilter
                        }, targets);

                        foreach (var target in targets)
                        {
                            if(effect.type == Effect.EffectType.Damage)
                            {
                                ApplyDamageEffect(target.entity, effects.source, modifiedEffect, position.value);
                            } else if(effect.type == Effect.EffectType.AreaDamage)
                            {
                                ApplyAreaDamageEffect(modifiedEffect.targeting.targetingFilter, target, effects.source, modifiedEffect, position.value);
                            }
                        }
                        
                        targets.Clear();
                    }

                }
            }
            
            foreach (var e in destroyableEffectsFilter.Value)
            {
                ref var destroyable = ref destroyableEffectsFilter.Pools.Inc2.Get(e);
                destroyable.destroy = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ApplyDamageEffect(Entity target, Entity source, Effect effect, Vector3 position)
        {
            ref var health = ref target.Get<HealthComponent>();
            health.damages.Add(new DamageData()
            {
                value = Random.Range(effect.minValue, effect.maxValue),
                position = position,
                knockback = false,
                source = source
            });
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ApplyAreaDamageEffect(TargetingFilter targetingFilter, Target target, Entity source, Effect effect, Vector3 position)
        {
            var distSqr = (target.position - position).sqrMagnitude;

            var factor = Mathf.Clamp01(distSqr / targetingFilter.maxRangeSqr);

            var damage = Mathf.Lerp(effect.maxValue, effect.minValue, factor);
            
            ref var health = ref target.entity.Get<HealthComponent>();
            health.damages.Add(new DamageData()
            {
                value = damage,
                position = position,
                knockback = false,
                source = source
            });
        }
    }
}