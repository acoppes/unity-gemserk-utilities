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
            
            foreach (var entity in filter.Value)
            {
                ref var healthRegenerationComponent = ref filter.Pools.Inc1.Get(entity);
                ref var healthComponent = ref filter.Pools.Inc2.Get(entity);
                
                if (!healthRegenerationComponent.enabled || healthComponent.IsFull())
                {
                    continue;
                }

                if (regenerationCooldown.IsReady)
                {
                    healthComponent.current += healthRegenerationComponent.deltaHealth;
                    if (healthComponent.current >= healthComponent.total)
                    {
                        healthComponent.current = healthComponent.total;
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