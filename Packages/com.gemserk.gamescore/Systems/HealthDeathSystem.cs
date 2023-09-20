using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;

namespace Game.Systems
{
    public class HealthDeathSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        public SignalAsset onEntityDeathSignal;
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<HealthComponent>(entity))
            {
                ref var healthComponent = ref world.GetComponent<HealthComponent>(entity);
                healthComponent.ClearEvents();
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var healthComponents = world.GetComponents<HealthComponent>();
            var destroyableComponents = world.GetComponents<DestroyableComponent>();
            
            foreach (var entity in world.GetFilter<HealthComponent>()
                         .Exc<DisabledComponent>().End())
            {
                var healthComponent = healthComponents.Get(entity);
                var worldEntity = world.GetEntity(entity);
                
                if (healthComponent.previousAliveState == HealthComponent.AliveType.Alive &&
                    healthComponent.aliveType == HealthComponent.AliveType.Death)
                {
                    healthComponent.OnDeathEvent(world, worldEntity);
                    
                    if (onEntityDeathSignal != null)
                    {
                        onEntityDeathSignal.Signal(worldEntity);
                    }
                }
            }
            
            foreach (var entity in world.GetFilter<HealthComponent>()
                         .Exc<DisabledComponent>().End())
            {
                var healthComponent = healthComponents.Get(entity);
                if (healthComponent.autoDisableOnDeath)
                {
                    if (healthComponent.previousAliveState == HealthComponent.AliveType.Alive &&
                        healthComponent.aliveType == HealthComponent.AliveType.Death)
                    {
                        world.AddComponent(world.GetEntity(entity), new DisabledComponent());
                    }
                }
            }
            
            foreach (var entity in world.GetFilter<HealthComponent>()
                         .Inc<DestroyableComponent>().End())
            {
                var healthComponent = healthComponents.Get(entity);
                ref var destroyableComponent = ref destroyableComponents.Get(entity);

                if (!healthComponent.autoDestroyOnDeath)
                {
                    continue;
                }
                
                if (healthComponent.previousAliveState == HealthComponent.AliveType.Alive &&
                    healthComponent.aliveType == HealthComponent.AliveType.Death)
                {
                    destroyableComponent.destroy = true;
                }
            }
        }


    }
}