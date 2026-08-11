using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class HealthAliveStateChangeControllerEventSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<HealthStateChangedEvent, ControllerComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var controllers = filter.Pools.Inc2.Get(e);
                
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