using System.Runtime.CompilerServices;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class HealthSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<HealthComponent>, Exc<DisabledComponent>> filter = default;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DamageData ProcessDamage(ref HealthComponent health, DamageData damage)
        {
            var mult = Mathf.Clamp01(1f - health.damageResistance);
            damage.value *= mult;
            
            health.current -= damage.value;
            
            if (health.current < 0)
            {
                damage.value += health.current;
                health.current = 0;
            }

            return damage;
        }
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = base.dt;
            
            foreach (var entity in filter.Value)
            {
                ref var health = ref filter.Pools.Inc1.Get(entity);
                
                health.temporaryInvulnerability.Decrease(deltaTime);
                health.processedDamages.Clear();
                
                health.timeSinceLastHit += deltaTime;

                health.previousAliveState = health.aliveType;
                
                var canReceiveDamage = health.temporaryInvulnerability.IsEmpty && !health.invulnerable;

                if (canReceiveDamage)
                {
                    for (var i = 0; i < health.damages.Count; i++)
                    {
                        var damage = health.damages[i];
                        health.timeSinceLastHit = 0;

                        damage = ProcessDamage(ref health, damage);
                        
                        // health.current -= damage.value;
                        //
                        // if (health.current < 0)
                        // {
                        //     damage.value += health.current;
                        //     health.current = 0;
                        // }
                        
                        health.processedDamages.Add(damage);
                        health.temporaryInvulnerability.Fill();

                        if (health.temporaryInvulnerability.Total > 0)
                        {
                            break;
                        }
                    }
                }

                for (var i = 0; i < health.healEffects.Count; i++)
                {
                    var effect = health.healEffects[i];
                    health.current += effect.value;
                    if (health.current > health.total)
                    {
                        health.current = health.total;
                    }
                }

                // if (health.processedDamages.Count > 0)
                // {
                //     health.OnDamageEvent(world, world.GetEntity(entity));
                // }
                
                health.damages.Clear();
                health.healEffects.Clear();

                if (health.current > 0 && health.triggerForceDeath)
                {
                    health.triggerForceDeath = false;
                    health.current = 0;
                }

                if (health.IsFull())
                {
                    health.timeInFullHealth += deltaTime;
                }
                else
                {
                    health.timeInFullHealth = 0;
                }
            }
        }
    }
}