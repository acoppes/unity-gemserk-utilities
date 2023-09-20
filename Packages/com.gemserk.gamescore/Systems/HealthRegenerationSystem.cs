using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class HealthRegenerationSystem : BaseSystem, IEcsRunSystem
    {
        public float regenerationTick = 4.0f / 15.0f;
        
        public void Run(EcsSystems systems)
        {
            var dt = Time.deltaTime;
            
            var healthComponents = world.GetComponents<HealthComponent>();
            var healthRegenerationComponents = world.GetComponents<HealthRegenerationComponent>();
            
            foreach (var entity in world.GetFilter<HealthRegenerationComponent>().End())
            {
                ref var healthRegenerationComponent = ref healthRegenerationComponents.Get(entity);
                healthRegenerationComponent.tick.SetTotal(regenerationTick);
            }
            
            foreach (var entity in world.GetFilter<HealthComponent>().Inc<HealthRegenerationComponent>().End())
            {
                ref var healthComponent = ref healthComponents.Get(entity);
                ref var healthRegenerationComponent = ref healthRegenerationComponents.Get(entity);
                
                healthRegenerationComponent.tick.SetTotal(regenerationTick);

                if (!healthRegenerationComponent.enabled || healthComponent.IsFull())
                {
                    healthRegenerationComponent.tick.Reset();
                    continue;
                }
                
                healthRegenerationComponent.tick.Increase(dt);

                if (healthRegenerationComponent.tick.IsReady)
                {
                    healthComponent.current += healthRegenerationComponent.regenerationPerTick;
                    if (healthComponent.current >= healthComponent.total)
                    {
                        healthComponent.current = healthComponent.total;
                    }
                    healthRegenerationComponent.tick.Reset();
                }
            }
        }
    }
}