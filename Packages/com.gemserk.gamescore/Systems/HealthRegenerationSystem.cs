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
            regenerationCooldown.Increase(dt);
            
            foreach (var e in filter.Value)
            {
                ref var regeneration = ref filter.Pools.Inc1.Get(e);
                ref var health = ref filter.Pools.Inc2.Get(e);
                
                if (!regeneration.enabled || health.IsFull())
                {
                    continue;
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
                        health.current += regeneration.deltaHealth;
                    }
                } else if (regeneration.regenerationType ==
                           HealthRegenerationComponent.RegenerationType.PerTime)
                {
                    health.current += regeneration.deltaHealth * dt;
                }
                
                if (health.current >= health.total)
                {
                    health.current = health.total;
                }
            }

            if (regenerationCooldown.IsReady)
            {
                regenerationCooldown.Reset();
            }
        }


    }
}