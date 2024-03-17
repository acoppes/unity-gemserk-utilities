using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class HealthAliveStateChangeControllerEventSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<HealthComponent, ControllerComponent, HealthAliveStateControllerEvent>, Exc<DisabledComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<HealthComponent>(entity) && world.HasComponent<ControllerComponent>(entity))
            {
                // if controller has IHealthStateChanged, cache it in this component.
                entity.Add<HealthAliveStateControllerEvent>();
            }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var health = filter.Pools.Inc1.Get(e);
                var controllers = filter.Pools.Inc2.Get(e);
                
                if (health.previousAliveState != health.aliveType)
                {
                    var entity = world.GetEntity(e);
                    foreach (var controller in controllers.controllers)
                    {
                        if (controller is IHealthStateChanged healthStateChanged)
                        {
                            healthStateChanged.OnHealthStateChanged(world, entity);
                        }
                    }
                }
            }
        }
    }
}