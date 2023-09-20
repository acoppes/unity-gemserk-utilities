using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class LimitPhysicsMaxFallingVelocitySystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public float maxVerticalVelocity = 1;

        private EcsFilter physicsFilter;
        private EcsFilter physics2dFilter;

        public void Init(EcsSystems systems)
        {
            physicsFilter = world.GetFilter<PhysicsComponent>()
                .Exc<DisabledComponent>()
                .End();
            
            physics2dFilter = world.GetFilter<Physics2dComponent>()
                .Exc<DisabledComponent>()
                .End();
        }
        
        public void Run(EcsSystems systems)
        {
            var physicsComponents = world.GetComponents<PhysicsComponent>();

            foreach (var entity in physicsFilter)
            {
                // copy from body to position`
                ref var physicsComponent = ref physicsComponents.Get(entity);

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
            
            var physics2dComponents = world.GetComponents<Physics2dComponent>();

            foreach (var entity in physics2dFilter)
            {
                // copy from body to position`
                ref var physics2dComponent = ref physics2dComponents.Get(entity);

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