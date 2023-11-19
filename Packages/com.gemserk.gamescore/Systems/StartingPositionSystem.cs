using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class StartingPositionSystem : BaseSystem, IEntityCreatedHandler
    {
        // readonly EcsFilterInject<Inc<PositionComponent, StartingPositionComponent>, 
        //     Exc<DisabledComponent, Physics2dComponent, PhysicsComponent>> filter = default;
        //
        // readonly EcsFilterInject<Inc<Physics2dComponent, StartingPositionComponent>, 
        //     Exc<DisabledComponent>> physics2dFilter = default;
        
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
                
                world.RemoveComponent<StartingPositionComponent>(e);
            }
        }
        
        // public void Run(EcsSystems systems)
        // {
        //     foreach (var e in filter.Value)
        //     {
        //         ref var position = ref filter.Pools.Inc1.Get(e);
        //         var startingPosition = filter.Pools.Inc2.Get(e);
        //
        //         position.value = startingPosition.value;
        //         
        //         world.RemoveComponent<StartingPositionComponent>(e);
        //     }
        //     
        //     foreach (var e in physics2dFilter.Value)
        //     {
        //         ref var physics2d = ref physics2dFilter.Pools.Inc1.Get(e);
        //         var startingPosition = physics2dFilter.Pools.Inc2.Get(e);
        //
        //         physics2d.body.position = startingPosition.value;
        //         
        //         world.RemoveComponent<StartingPositionComponent>(e);
        //     }
        //     
        // }


    }
}