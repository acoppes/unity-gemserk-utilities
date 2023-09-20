using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class MovementController : ControllerBase, IUpdate, IInit
    {
        public void OnInit(World world, Entity entity)
        {
            if (entity.Has<MovementComponent>())
            {
                ref var movementComponent = ref entity.Get<MovementComponent>();
                movementComponent.speed = movementComponent.baseSpeed;
            }
        }

        public void OnUpdate(World world, Entity entity, float dt)
        {
            if (entity.Has<PhysicsComponent>())
            {
                if (entity.Get<PhysicsComponent>().syncType == PhysicsComponent.SyncType.FromPhysics)
                {
                    return;
                }
            }

            ref var states = ref entity.Get<StatesComponent>();
            
            if (entity.Get<ActiveControllerComponent>().IsControlled())
            {
                states.ExitState("Moving");
                return;
            }

            ref var animations = ref entity.Get<AnimationComponent>();
            ref var input = ref entity.Get<InputComponent>();
            
            var walkAnimation = 
                animations.animationsAsset.GetDirectionalAnimation("Walk", input.direction().vector2);
            
            if (entity.Has<MovementComponent>())
            {
                if (entity.Get<MovementComponent>().speed < 0.01f)
                {
                    return;
                }

                // movement.speed = movement.baseSpeed;
                entity.Get<MovementComponent>().movingDirection = input.direction3d();

                if (states.HasState("Moving"))
                {
                    if (!animations.IsPlaying(walkAnimation))
                    {
                        animations.Play(walkAnimation);
                    }

                    if (input.backward().isPressed)
                    {
                        entity.Get<LookingDirection>().value.x = input.direction().vector2.x;
                    }

                    if (input.direction().vector2.sqrMagnitude < Mathf.Epsilon)
                    {
                        states.ExitState("Moving");
                        return;
                    }

                    entity.Get<LookingDirection>().value = input.direction3d().normalized;

                    return;
                }
            }
            
            if (input.direction().vector2.sqrMagnitude > Mathf.Epsilon)
            {
                if (input.backward().isPressed)
                {
                    entity.Get<LookingDirection>().value.x = input.direction().vector2.x;
                }
                
                animations.Play(walkAnimation);
                states.EnterState("Moving");
                
                return;
            }

            if (!animations.IsPlaying("Idle"))
            {
                animations.Play("Idle");
            }
        }

    }
}