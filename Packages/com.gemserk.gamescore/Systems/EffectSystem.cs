using System.Collections.Generic;
using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

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
                    if (effect.targetType == Effect.TargetType.Target)
                    {
                        if (effects.target != null && effects.target.entity.Exists())
                        {
                            ApplyDamageEffect(effects.target.entity, effects.source, effect, position.value);
                        }
                    }

                    if (effect.targetType == Effect.TargetType.Source)
                    {
                        if (effects.source.Exists())
                        {
                            ApplyDamageEffect(effects.source, effects.source, effect, position.value);
                        }
                    }

                    if (effect.targetType == Effect.TargetType.TargetsFromTargeting)
                    {
                        world.GetTargets(new RuntimeTargetingParameters()
                        {
                            alliedPlayersBitmask = player.GetAlliedPlayers(),
                            position = position.value,
                            direction = new Vector3(1, 0, 0),
                            filter = effect.targeting.targetingFilter
                        }, targets);

                        foreach (var target in targets)
                        {
                            ApplyDamageEffect(target.entity, effects.source, effect, position.value);
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

        private void ApplyDamageEffect(Entity target, Entity source, Effect effect, Vector3 position)
        {
            ref var health = ref target.Get<HealthComponent>();
            health.damages.Add(new DamageData()
            {
                value = effect.value,
                position = position,
                knockback = false,
                source = source
            });
        }
    }
}