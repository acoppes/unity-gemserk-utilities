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

            var regenPerTickCooldownReady = regenerationCooldown.IsReady;
            
            foreach (var e in filter.Value)
            {
                ref var regeneration = ref filter.Pools.Inc1.Get(e);
                ref var health = ref filter.Pools.Inc2.Get(e);

                regeneration.wasActive = regeneration.isActive;
                regeneration.isActive = false;
                
                if (!regeneration.enabled || health.IsFull())
                {
                    regeneration.regenerationDelayCurrent = 0;
                    continue;
                }

                if (regeneration.regenerationDelayTotal > 0)
                {
                    regeneration.regenerationDelayCurrent += deltaTime;
                    
                    if (health.processedDamages.Count > 0)
                    {
                        // TODO: could check if damges did damage or not...
                        regeneration.regenerationDelayCurrent = 0;
                    }
                    
                    if (regeneration.regenerationDelayCurrent < regeneration.regenerationDelayTotal)
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
                    if (health.current < health.total)
                    {
                        regeneration.isActive = true;
                 
                        if (regenPerTickCooldownReady)
                        {
                            health.current += regeneration.regeneration;
                            if (health.current > health.total)
                            {
                                health.current = health.total;
                            }
                        }
                    }
                } else if (regeneration.regenerationType ==
                           HealthRegenerationComponent.RegenerationType.PerTime)
                {
                    var regenValue = regeneration.regeneration * deltaTime;
                    if (health.current < health.total)
                    {
                        regeneration.isActive = true;
                        
                        health.current += regenValue;
                        if (health.current > health.total)
                        {
                            health.current = health.total;
                        }
                    }
                }
            }

            if (regenPerTickCooldownReady)
            {
                regenerationCooldown.Reset();
            }
        }


    }
}