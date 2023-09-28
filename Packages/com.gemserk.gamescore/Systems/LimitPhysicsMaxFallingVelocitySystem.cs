using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class LimitPhysicsMaxFallingVelocitySystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PhysicsComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, ConfigurationComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
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

                var bodyVelocity = physicsComponent.body.velocity;
                
                var velocityY = bodyVelocity.y;
                var magnitude = Mathf.Abs(velocityY);
                
                if (velocityY < 0 && magnitude > maxVerticalVelocity)
                {
                    var velocity = bodyVelocity;
                    velocity.y = maxVerticalVelocity * (velocityY / magnitude);
                    physicsComponent.body.velocity = velocity;
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

                var body = physics2dComponent.body;
                var bodyVelocity = body.velocity;
                
                var velocityY = bodyVelocity.y;
                var magnitude = Mathf.Abs(velocityY);
                
                if (velocityY < 0 && magnitude > maxVerticalVelocity)
                {
                    var velocity = bodyVelocity;
                    velocity.y = maxVerticalVelocity * (velocityY / magnitude);
                    body.velocity = velocity;
                }
            }
        }


    }
}