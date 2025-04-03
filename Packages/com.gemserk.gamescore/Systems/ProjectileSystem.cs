using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class ProjectileSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ProjectileComponent, ProjectileFireComponent>, Exc<DisabledComponent>> 
            pendingProjectiles = default;
        
        readonly EcsFilterInject<Inc<ProjectileComponent, PositionComponent, PhysicsComponent, LookingDirection>, Exc<DisabledComponent>> 
            projectilesPhysicsFilter = default;
        
        readonly EcsFilterInject<Inc<ProjectileComponent, PositionComponent, Physics2dComponent, LookingDirection>, Exc<DisabledComponent>> 
            projectilesPhysics2dFilter = default;
        
        readonly EcsFilterInject<Inc<ProjectileComponent>, Exc<DisabledComponent>> 
            projectilesFilter = default;
        
        readonly EcsFilterInject<Inc<ProjectileComponent, DestroyableComponent>, Exc<DisabledComponent, HealthComponent>> 
            destroyablesFilter = default;
        
        readonly EcsFilterInject<Inc<ProjectileComponent, HealthComponent>, Exc<DisabledComponent>> 
            destroyWithHealth = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in pendingProjectiles.Value)
            {
                ref var projectile = ref pendingProjectiles.Pools.Inc1.Get(e);
                ref var projectileFire = ref pendingProjectiles.Pools.Inc2.Get(e);

                projectile.initialVelocity = projectileFire.direction;
                projectile.state = ProjectileComponent.State.Pending;
                
                pendingProjectiles.Pools.Inc2.Del(e);
                // world.RemoveComponent<ProjectileFireComponent>(e);
            }

            // play vfx animation if didn't start yet
            foreach (var entity in projectilesPhysicsFilter.Value)
            {
                ref var projectileComponent = ref projectilesPhysicsFilter.Pools.Inc1.Get(entity);
                var positionComponent = projectilesPhysicsFilter.Pools.Inc2.Get(entity);
                ref var physicsComponent = ref projectilesPhysicsFilter.Pools.Inc3.Get(entity);
                ref var lookingDirectionComponent = ref projectilesPhysics2dFilter.Pools.Inc4.Get(entity);
                
                if (projectileComponent.state == ProjectileComponent.State.Pending)
                {
                    projectileComponent.state = ProjectileComponent.State.Traveling;
                    
                    projectileComponent.travelTime = 0;

                    physicsComponent.syncType = PhysicsComponent.SyncType.FromPhysics;
                    
                    var initialPosition = positionComponent.value + 
                                                           projectileComponent.initialVelocity.normalized * projectileComponent.initialOffset;
                    
                    physicsComponent.body.position = initialPosition;
                    physicsComponent.velocity = projectileComponent.initialVelocity;

                    if (projectileComponent.trajectoryType == ProjectileComponent.TrajectoryType.Linear)
                    {
                        physicsComponent.velocity = projectileComponent.initialVelocity * projectileComponent.initialSpeed;
                    }
                    
                    lookingDirectionComponent.value = projectileComponent.initialVelocity.normalized;

                    physicsComponent.body.constraints = RigidbodyConstraints.None;
                    
                    projectileComponent.previousPosition = initialPosition;
                    
                    continue;
                }
                
                if (projectileComponent.state == ProjectileComponent.State.Traveling)
                {
                    if (projectileComponent.maxDistance > 0)
                    {
                        projectileComponent.travelDistance += Vector3.Distance(physicsComponent.body.position,
                            projectileComponent.previousPosition);
                    }
                    
                    projectileComponent.travelTime += dt;
                    
                    var velocity = physicsComponent.velocity;

                    var moving = velocity.sqrMagnitude > 0.1f;
                    if (moving)
                    {
                        lookingDirectionComponent.value = velocity.normalized;
                    }

                    projectileComponent.previousPosition = physicsComponent.body.position;
                }
            }
            
            foreach (var entity in projectilesPhysics2dFilter.Value)
            {
                ref var projectileComponent = ref projectilesPhysics2dFilter.Pools.Inc1.Get(entity);
                var positionComponent = projectilesPhysics2dFilter.Pools.Inc2.Get(entity);
                ref var physicsComponent = ref projectilesPhysics2dFilter.Pools.Inc3.Get(entity);
                ref var lookingDirectionComponent = ref projectilesPhysics2dFilter.Pools.Inc4.Get(entity);
                
                if (projectileComponent.state == ProjectileComponent.State.Pending)
                {
                    projectileComponent.state = ProjectileComponent.State.Traveling;
                    
                    projectileComponent.travelTime = 0;

                    physicsComponent.syncType = PhysicsComponent.SyncType.FromPhysics;
                    
                    var initialPosition = positionComponent.value + 
                                                           projectileComponent.initialVelocity.normalized * projectileComponent.initialOffset;
                    
                    physicsComponent.body.position = initialPosition;
                    physicsComponent.velocity = projectileComponent.initialVelocity;
                    
                    if (projectileComponent.trajectoryType == ProjectileComponent.TrajectoryType.Linear)
                    {
                        physicsComponent.velocity = projectileComponent.initialVelocity * projectileComponent.initialSpeed;
                    }

                    physicsComponent.body.constraints = RigidbodyConstraints2D.None;
                    
                    projectileComponent.previousPosition = initialPosition;
                    
                    continue;
                }

                if (projectileComponent.state == ProjectileComponent.State.Traveling)
                {
                    if (projectileComponent.maxDistance > 0)
                    {
                        projectileComponent.travelDistance += Vector3.Distance(physicsComponent.body.position,
                            projectileComponent.previousPosition);
                    }
                    
                    projectileComponent.travelTime += dt;
                    
                    var velocity = physicsComponent.velocity;

                    var moving = velocity.sqrMagnitude > 0.1f;
                    if (moving)
                    {
                        lookingDirectionComponent.value = velocity.normalized;
                    }

                    projectileComponent.previousPosition = physicsComponent.body.position;
                }
            }
            
            foreach (var e in projectilesFilter.Value)
            {
                ref var projectile = ref projectilesFilter.Pools.Inc1.Get(e);
                projectile.wasImpacted = projectile.impacted;
            }
            
            foreach (var entity in destroyablesFilter.Value)
            {
                var projectileComponent = destroyablesFilter.Pools.Inc1.Get(entity);

                if (projectileComponent.maxDistance > 0 && projectileComponent.travelDistance > projectileComponent.maxDistance)
                {
                    ref var destroyable = ref destroyablesFilter.Pools.Inc2.Get(entity);
                    destroyable.destroy = true;
                }
            }
            
            foreach (var entity in destroyWithHealth.Value)
            {
                var projectileComponent = destroyWithHealth.Pools.Inc1.Get(entity);

                if (projectileComponent.maxDistance > 0 && projectileComponent.travelDistance > projectileComponent.maxDistance)
                {
                    ref var health = ref destroyWithHealth.Pools.Inc2.Get(entity);
                    health.triggerForceDeath = true;
                }
            }
        }
    }
}