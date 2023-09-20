using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class CopyPositionFromPhysicsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PhysicsComponent, PositionComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, PositionComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in physicsFilter.Value)
            {
                // copy from body to position`
                ref var physicsComponent = ref physicsFilter.Pools.Inc1.Get(entity);

                if (physicsComponent.isStatic)
                {
                    continue;
                }
                
                ref var positionComponent = ref physicsFilter.Pools.Inc2.Get(entity);

                positionComponent.value = physicsComponent.body.position;
                physicsComponent.velocity = physicsComponent.body.velocity;
            }
            
            foreach (var entity in physics2dFilter.Value)
            {
                // copy from body to position`
                ref var physics2dComponent = ref physics2dFilter.Pools.Inc1.Get(entity);

                if (physics2dComponent.isStatic)
                {
                    continue;
                }
                
                ref var positionComponent = ref physics2dFilter.Pools.Inc2.Get(entity);

                positionComponent.value = physics2dComponent.body.position;
                // physicsComponent.velocity = physics2dComponent.body.velocity;
            }
        }
    }
}