using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class StartingPositionSystem : BaseSystem, IEntityCreatedHandler
    {
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
    }
}