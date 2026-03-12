using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public interface IRespawnedEvent : IControllerEvent
    {
        void OnRespawned(World world, Entity entity);
    }
    
    public class OnRespawnedEventControllerSystem : BaseSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ControllerComponent, RespawnEventComponent>> respawned = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in respawned.Value)
            {
                var controllers = respawned.Pools.Inc1.Get(e);
                foreach (var controller in controllers.controllers)
                {
                    if (controller is IRespawnedEvent respawnedEvent)
                    {
                        respawnedEvent.OnRespawned(world, world.GetEntity(e));
                    }
                }
            }
        }
    }
}