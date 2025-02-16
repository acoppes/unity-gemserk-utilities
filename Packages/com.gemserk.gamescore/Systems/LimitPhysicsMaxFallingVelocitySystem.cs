using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class LimitPhysicsMaxFallingVelocitySystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PhysicsComponent, LimitMaxFallingVelocityComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, LimitMaxFallingVelocityComponent, ConfigurationComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public float maxVerticalVelocity = 1;
        
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

                var bodyVelocity = physicsComponent.velocity;
                
                var velocityY = bodyVelocity.y;
                var magnitude = Mathf.Abs(velocityY);
                
                if (velocityY < 0 && magnitude > maxVerticalVelocity)
                {
                    var velocity = bodyVelocity;
                    velocity.y = maxVerticalVelocity * (velocityY / magnitude);
                    physicsComponent.velocity = velocity;
                }
            }

            foreach (var entity in physics2dFilter.Value)
            {
                // copy from body to position`
                ref var physics2dComponent = ref physics2dFilter.Pools.Inc1.Get(entity);

                if (physics2dComponent.isStatic)
                {
                    continue;
                }

                var bodyVelocity = physics2dComponent.velocity;
                
                var velocityY = bodyVelocity.y;
                var magnitude = Mathf.Abs(velocityY);
                
                if (velocityY < 0 && magnitude > maxVerticalVelocity)
                {
                    var velocity = bodyVelocity;
                    velocity.y = maxVerticalVelocity * (velocityY / magnitude);
                    physics2dComponent.velocity = velocity;
                }
            }
        }


    }
}