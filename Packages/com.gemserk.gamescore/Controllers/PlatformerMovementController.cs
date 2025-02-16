using Game.Components;
using Game.Components.Abilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using MyBox;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Controllers
{
    public class PlatformerMovementController : ControllerBase, IUpdate, IActiveController
    {
        public LayerMask groundDetectionMask;
        
        private RaycastHit2D[] raycasts = new RaycastHit2D[1];

        private void EnterJumping(World world, Entity entity)
        {
            entity.Get<GravityComponent>().disabled = true;
            
            entity.Get<StatesComponent>().EnterState("Jumping");
            entity.Get<StatesComponent>().EnterState("Jumping/GoingUp");

            var jumpAbility = entity.GetAbilitiesComponent().GetAbility("Jump");
            jumpAbility.Start();
            
            // start jumping first frame
            var physics2dComponent = entity.Get<Physics2dComponent>();
            var velocity = physics2dComponent.velocity;
            
            var jumpComponent = entity.GetJumpComponent();
            velocity.y = 1 * jumpComponent.speed;
            physics2dComponent.velocity = velocity;
        }
        
        private void ExitJumping(Entity entity)
        {
            entity.Get<StatesComponent>().ExitStatesAndSubStates("Jumping");
            entity.Get<GravityComponent>().disabled = false;
            
            var jumpAbility = entity.GetAbilitiesComponent().GetAbility("Jump");
            jumpAbility.Stop(Ability.StopType.Completed);
        }
        
        private void EnterFalling(Entity entity)
        {
            entity.Get<StatesComponent>().EnterState("Falling");

            if (entity.Get<Physics2dComponent>().body.linearVelocity.y > 0)
            {
                entity.Get<StatesComponent>().EnterState("Falling/GoingUp");
            }

            ref var platformer = ref entity.GetPlatformerComponent();
            platformer.fallingTime = 0;
            
            if (platformer.fallingConsumesGroundJump)
            {
                var jumpAbility = entity.GetAbilitiesComponent().GetAbility("Jump");
                jumpAbility.ConsumeCharge(jumpAbility.totalCharges);
                
                // if (jumpComponent.currentJump < 1)
                // {
                //     jumpComponent.currentJump = 1;
                // }
            }
        }
        
        private void ExitFalling(Entity entity)
        {
            entity.Get<StatesComponent>().ExitStatesAndSubStates("Falling");
            entity.Get<GravityComponent>().disabled = false;
        }

        public bool CanBeInterrupted(Entity entity, IActiveController activeController)
        {
            return true;
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            if (entity.Get<StatesComponent>().HasState("Jumping"))
            {
                ExitJumping(entity);
            }
            
            if (entity.Get<StatesComponent>().HasState("Falling"))
            {
                ExitFalling(entity);
            }
        }

        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var platformer = ref entity.GetPlatformerComponent();
            var gravityComponent = entity.Get<GravityComponent>();

            var jumpAbility = entity.GetAbilitiesComponent().GetAbility("Jump");
            var physics2dComponent = entity.Get<Physics2dComponent>();
            
            var velocity = physics2dComponent.body.linearVelocity;

            velocity.x = 0;

            var control = entity.Get<InputComponent>();
            ref var bufferedInput = ref entity.Get<BufferedInputComponent>();
            
            var states = entity.Get<StatesComponent>();

            // var crouch = states.HasState("Crouch");

            ref var statesController = ref entity.Get<ActiveControllerComponent>();
            
            if (!statesController.IsControlled(this))
            {
                if (!statesController.CanInterrupt(entity, this))
                {
                    return;
                }
                statesController.TakeControl(entity, this);
            }

            var platformerSpeed = platformer.GetSpeed(gravityComponent.inContactWithGround);
            
            if (control.right().isPressed)
            {
                velocity.x = 1 * platformerSpeed;
                entity.Get<LookingDirection>().value = new Vector3(1, 0, 0);
            }
            
            if (control.left().isPressed)
            {
                velocity.x = -1  * platformerSpeed;
                entity.Get<LookingDirection>().value = new Vector3(-1, 0, 0);
            }

            physics2dComponent.body.linearVelocity = velocity;
            
            State state;
            ref var jumpComponent = ref entity.GetJumpComponent();
            
            // jumpComponent.airJumpDelay.Increase(dt);

            if (states.TryGetState("Falling", out state))
            {
                platformer.fallingTime += dt;
                
                if (states.HasState("Falling/GoingUp"))
                {
                    if (entity.Get<Physics2dComponent>().body.linearVelocity.y <= 0)
                    {
                        states.ExitState("Falling/GoingUp");
                    }
                }

                if (bufferedInput.HasBufferedAction(control.button1()) && jumpAbility.cooldown.IsReady)
                {
                    if (platformer.fallingJumpGroundDetectionOffset > 0)
                    {
                        var offset = platformer.fallingJumpGroundDetectionOffset;

                        var contactFilter = new ContactFilter2D()
                        {
                            useLayerMask = true,
                            layerMask = groundDetectionMask,
                            useTriggers = false
                        };

                        var count = DrawPhysics2D.Raycast(entity.Get<PositionComponent>().value.ToVector2(), Vector2.down,
                            contactFilter, raycasts, offset);

                        if (count >= 1)
                        {
                            jumpAbility.Fill();

                            var abilities = entity.GetAbilitiesComponent();
                            var dashAbility = abilities.GetAbility("Dash");

                            if (dashAbility != null)
                            {
                                dashAbility.Fill();
                            }

                            // if (entity.Has<DashComponent>())
                            // {
                            //     entity.GetDashComponent().availableDashCount = 1;
                            // }
                        }
                    }

                    if (jumpAbility.isReady)
                    {
                        bufferedInput.ConsumeBuffer();
                        
                        ExitFalling(entity);
                        EnterJumping(world, entity);
                        
                        return;
                    }
                }

                if (gravityComponent.inContactWithGround)
                {
                    jumpAbility.Fill();
                    
                    // jumpComponent.currentJump = 0;
                    // jumpComponent.airJumpDelay.Reset();
                    
                    ExitFalling(entity);
                }

                return;
            }

            if (states.TryGetState("Jumping", out state))
            {
                if (!control.button1().isPressed)
                {
                    states.ExitState("Jumping/GoingUp");
                }

                var jumping = state.time < jumpComponent.minTime;
               // Debug.Log($"jumping: {jumping} , {state.time}, {jumpComponent.minTime}");

                // var t = Mathf.InverseLerp(0, jumpComponent.maxTime, jumpAbility.activeTime);

                if (states.HasState("Jumping/GoingUp") && control.button1().isPressed)
                {
                    jumping = true;
                }

                if (jumping)
                {
                    velocity.y = 1 * jumpComponent.speed;
                }

                physics2dComponent.body.linearVelocity = velocity;
                
                if (state.time > jumpComponent.maxTime || !jumping)
                {
                    ExitJumping(entity);
                    EnterFalling(entity);
                }

                return;
            }

            if (gravityComponent.inContactWithGround)
            {
                jumpAbility.Fill();
                
                // jumpComponent.currentJump = 0;
                // jumpComponent.airJumpDelay.Reset();
            }

            var jumpPressed = bufferedInput.HasBufferedAction(control.button1());
            
            if (jumpPressed && jumpAbility.isReady)
            {
                // Debug.Log($"{Time.frameCount}: TOCASTE SALTO");
                var canJump = true;

                if (jumpComponent.jumpBufferTime > 0)
                {
                    canJump = gravityComponent.timeSinceGroundContact < jumpComponent.jumpBufferTime;
                }
                
                if (canJump)
                {
                    bufferedInput.ConsumeBuffer();
                    
                    // states.EnterState("Jumping");
                    EnterJumping(world, entity);
                    
                    return;
                }
            }

            if (!gravityComponent.inContactWithGround && gravityComponent.timeSinceGroundContact >= jumpComponent.jumpBufferTime)
            {
                EnterFalling(entity);
            }
            
        }
    }
}
