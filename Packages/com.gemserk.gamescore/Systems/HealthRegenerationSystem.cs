using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class HealthRegenerationSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<HealthRegenerationComponent, HealthComponent>, Exc<DisabledComponent>> 
            filter = default;
        
        public float regenerationTick = 4.0f / 15.0f;

        private Cooldown regenerationCooldown;
        
        public void Init(EcsSystems systems)
        {
            regenerationCooldown = new Cooldown(regenerationTick);
        }
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;
            
            regenerationCooldown.Increase(deltaTime);
            
            foreach (var e in filter.Value)
            {
                ref var regeneration = ref filter.Pools.Inc1.Get(e);
                ref var health = ref filter.Pools.Inc2.Get(e);
                
                if (!regeneration.enabled || health.IsFull())
                {
                    continue;
                }

                if (regeneration.damageRegenerationDisableTime > 0)
                {
                    regeneration.damageRegenerationDisableCurrent += deltaTime;
                    
                    if (health.processedDamages.Count > 0)
                    {
                        // TODO: could check if damges did damage or not...
                        regeneration.damageRegenerationDisableCurrent = 0;
                    }
                    
                    if (regeneration.damageRegenerationDisableCurrent < regeneration.damageRegenerationDisableTime)
                    {
                        continue;
                    }
                }

                if (health.aliveType != HealthComponent.AliveType.Alive)
                {
                    continue;
                }

                if (regeneration.regenerationType ==
                    HealthRegenerationComponent.RegenerationType.PerTick)
                {
                    if (regenerationCooldown.IsReady)
                    {
                        health.current += regeneration.regeneration;
                        if (health.current > health.total)
                        {
                            health.current = health.total;
                        }
                    }
                } else if (regeneration.regenerationType ==
                           HealthRegenerationComponent.RegenerationType.PerTime)
                {
                    var regenValue = regeneration.regeneration * deltaTime;
                    if (health.current < health.total)
                    {
                        health.current += regenValue;
                        if (health.current > health.total)
                        {
                            health.current = health.total;
                        }
                    }
                    
                }
            }

            if (regenerationCooldown.IsReady)
            {
                regenerationCooldown.Reset();
            }
        }


    }
}