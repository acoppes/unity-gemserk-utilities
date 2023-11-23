using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class HealthSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<HealthComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var health = ref filter.Pools.Inc1.Get(entity);
                var worldEntity = world.GetEntity(entity);

                health.temporaryInvulnerability.Decrease(dt);
                health.processedDamages.Clear();
                
                health.timeSinceLastHit += dt;

                health.previousAliveState = health.aliveType;
                
                var canReceiveDamage = health.temporaryInvulnerability.IsEmpty && !health.invulnerable;

                if (canReceiveDamage)
                {
                    foreach (var damage in health.damages)
                    {
                        health.timeSinceLastHit = 0;
                        health.current -= damage.value;
                        health.processedDamages.Add(damage);
                        health.temporaryInvulnerability.Fill();

                        if (health.temporaryInvulnerability.Total > 0)
                        {
                            break;
                        }
                    }
                }
                
                foreach (var effect in health.healEffects)
                {
                    health.current += effect.value;
                    if (health.current > health.total)
                    {
                        health.current = health.total;
                    }
                }
                
                if (health.processedDamages.Count > 0)
                {
                    health.OnDamageEvent(world, worldEntity);
                }
                
                health.damages.Clear();
                health.healEffects.Clear();

                if (health.current > 0 && health.triggerForceDeath)
                {
                    health.triggerForceDeath = false;
                    health.current = 0;
                }

                if (health.IsFull())
                {
                    health.timeInFullHealth += dt;
                }
                else
                {
                    health.timeInFullHealth = 0;
                }
            }
        }
    }
}