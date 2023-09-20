using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class PhysicsGravitySystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<GravityComponent, PhysicsComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<GravityComponent, Physics2dComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public static Vector3 gravity = new Vector3(0, -9.81f, 0);

        public void Init(EcsSystems systems)
        {
            Physics.gravity = Vector3.zero;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in physicsFilter.Value)
            {
                ref var gravityComponent = ref physicsFilter.Pools.Inc1.Get(entity);
                ref var physicsComponent = ref physicsFilter.Pools.Inc2.Get(entity);

                var inContactWithGround = false;
                    
                // if (gravityComponent.disabled)
                // {
                //     continue;
                // }

                if (physicsComponent.isStatic)
                {
                    gravityComponent.inContactWithGround = false;
                    continue;
                }
                
                gravityComponent.groundContactTime += dt;
                gravityComponent.timeSinceGroundContact += dt;
                
                var fallDirection = gravity.normalized;

                ContactPoint groundContact = default;

                for (var i = 0; i < physicsComponent.contactsCount; i++)
                {
                    var contactPoint = physicsComponent.contacts[i];
                    var dot = Vector3.Dot(fallDirection, contactPoint.normal);
                    // should be in the opposite direction
                    if (dot < -0.9f)
                    {
                        inContactWithGround = true;
                        groundContact = contactPoint;
                        break;
                    }
                }

                // foreach (var collision in physicsComponent.collisions)
                // {
                //     // check normal against gravity direction
                //     foreach (var contactPoint in collision.contacts)
                //     {
                //         var dot = Vector3.Dot(fallDirection, contactPoint.normal);
                //         // should be in the opposite direction
                //         if (dot < -0.9f)
                //         {
                //             gravityComponent.inContactWithGround = true;
                //             groundContact = contactPoint;
                //             break;
                //         }
                //     }
                //
                //     if (gravityComponent.inContactWithGround)
                //     {
                //         break;
                //     }
                // }

                if (!inContactWithGround)
                {
                    gravityComponent.groundContactTime = 0;
                }
                else
                {
                    gravityComponent.timeSinceGroundContact = 0;
                }
                
                if (!gravityComponent.disabled)
                {
                    physicsComponent.body.AddForce(gravity * gravityComponent.scale, ForceMode.Acceleration);
                    
                    if (inContactWithGround)
                    {
                        physicsComponent.body.position = physicsComponent.body.position.SetY(groundContact.point.y);
                    }
                }

                gravityComponent.inContactWithGround = inContactWithGround;
            }
            
            foreach (var entity in physics2dFilter.Value)
            {
                ref var gravityComponent = ref physics2dFilter.Pools.Inc1.Get(entity);
                ref var physics2dComponent = ref physics2dFilter.Pools.Inc2.Get(entity);

                gravityComponent.inContactWithGround = false;

                if (physics2dComponent.isStatic)
                {
                    continue;
                }
                
                gravityComponent.groundContactTime += dt;
                gravityComponent.timeSinceGroundContact += dt;
                
                var fallDirection = gravity.normalized;
                
                ContactPoint2D groundContact = default;

                foreach (var contactPoint in physics2dComponent.contacts)
                {
                    var dot = Vector3.Dot(fallDirection, contactPoint.normal);
                    // should be in the opposite direction
                    if (dot < -0.6f)
                    {
                        gravityComponent.inContactWithGround = true;
                        groundContact = contactPoint;
                        break;
                    }
                }

                if (!gravityComponent.inContactWithGround)
                {
                    gravityComponent.groundContactTime = 0;
                }
                else
                {
                    gravityComponent.timeSinceGroundContact = 0;
                }
                
                if (!gravityComponent.disabled)
                {
                    physics2dComponent.body.gravityScale = gravityComponent.scale;
                    
                    // physics2dComponent.body.AddForce(gravity * gravityComponent.scale, ForceMode2D.Force);
                    
                    if (gravityComponent.inContactWithGround)
                    {
                        physics2dComponent.body.position = physics2dComponent.body.position.SetY(groundContact.point.y);
                    }
                }
                else
                {
                    physics2dComponent.body.gravityScale = 0;
                }
            }
        }
    }
}