using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class HealthDeathSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<HealthComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<HealthComponent, DestroyableComponent>> destroyableFilter = default;
        
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
            foreach (var entity in filter.Value)
            {
                var healthComponent = filter.Pools.Inc1.Get(entity);
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
            
            foreach (var entity in filter.Value)
            {
                var healthComponent = filter.Pools.Inc1.Get(entity);
                if (healthComponent.autoDisableOnDeath)
                {
                    if (healthComponent.previousAliveState == HealthComponent.AliveType.Alive &&
                        healthComponent.aliveType == HealthComponent.AliveType.Death)
                    {
                        world.AddComponent(world.GetEntity(entity), new DisabledComponent());
                    }
                }
            }
            
            foreach (var entity in destroyableFilter.Value)
            {
                var healthComponent = destroyableFilter.Pools.Inc1.Get(entity);
                ref var destroyableComponent = ref destroyableFilter.Pools.Inc2.Get(entity);

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