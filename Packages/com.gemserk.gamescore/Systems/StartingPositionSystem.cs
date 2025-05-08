using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class StartingPositionSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, StartingPositionComponent>, Exc<DisabledComponent>> positionFilter = default;
        readonly EcsFilterInject<Inc<GameObjectComponent, StartingPositionComponent>, Exc<DisabledComponent>> gameObjectFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, StartingPositionComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<StartingPositionComponent>, Exc<DisabledComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity e)
        {
            if (world.HasComponent<StartingPositionComponent>(e))
            {
                var startingPosition = e.Get<StartingPositionComponent>();
               
                ref var position = ref e.Get<PositionComponent>();
                position.value = startingPosition.value;

                if (e.Has<Physics2dComponent>())
                {
                    ref var physicsComponent = ref e.Get<Physics2dComponent>();
                    physicsComponent.body.position = startingPosition.value;
                }
                
                if (e.Has<GameObjectComponent>())
                {
                    ref var gameObjectComponent = ref e.Get<GameObjectComponent>();
                    gameObjectComponent.gameObject.transform.position = startingPosition.value;
                }
                
                world.RemoveComponent<StartingPositionComponent>(e);
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var e in positionFilter.Value)
            {
                ref var position = ref positionFilter.Pools.Inc1.Get(e);
                var startingPosition = positionFilter.Pools.Inc2.Get(e);
                position.value = startingPosition.value;
            }
            
            foreach (var e in gameObjectFilter.Value)
            {
                ref var gameObjectComponent = ref gameObjectFilter.Pools.Inc1.Get(e);
                var startingPosition = gameObjectFilter.Pools.Inc2.Get(e);
                gameObjectComponent.gameObject.transform.position = startingPosition.value;
            }
            
            foreach (var e in physicsFilter.Value)
            {
                ref var physicsComponent = ref physicsFilter.Pools.Inc1.Get(e);
                var startingPosition = physicsFilter.Pools.Inc2.Get(e);
                physicsComponent.body.position = startingPosition.value;
            }
            
            foreach (var e in filter.Value)
            {
                filter.Pools.Inc1.Del(e);
            }
        }
    }
}