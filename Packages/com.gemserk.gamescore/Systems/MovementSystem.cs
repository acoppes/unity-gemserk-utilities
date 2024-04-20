using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class MovementSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        readonly EcsFilterInject<Inc<MovementComponent, PositionComponent>, Exc<DisabledComponent>> stationaryFilter = default;
        readonly EcsFilterInject<Inc<MovementComponent, PositionComponent>, Exc<DisabledComponent, PhysicsMovementComponent>> filter = default;
        
        public float distanceToConsiderStationary = 0.01f;

        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<MovementComponent>())
            {
                ref var movement = ref entity.Get<MovementComponent>();
                if (movement.initialSpeedType == MovementComponent.InitialSpeedType.BaseSpeed)
                {
                    movement.speed = movement.baseSpeed;
                }
            }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in stationaryFilter.Value)
            {
                ref var movement = ref stationaryFilter.Pools.Inc1.Get(entity);
                ref var position = ref stationaryFilter.Pools.Inc2.Get(entity);
                
                // first check if didn't move last time
                if ((position.value - movement.previousPosition).sqrMagnitude < distanceToConsiderStationary * distanceToConsiderStationary)
                {
                    movement.stationaryTime += dt;
                }
                else
                {
                    movement.stationaryTime = 0;
                }
                
                movement.previousPosition = position.value;
            }

            foreach (var entity in filter.Value)
            {
                ref var movement = ref filter.Pools.Inc1.Get(entity);
                ref var position = ref filter.Pools.Inc2.Get(entity);
                
                // first check if didn't move last time
                if ((position.value - movement.previousPosition).sqrMagnitude < distanceToConsiderStationary * distanceToConsiderStationary)
                {
                    movement.stationaryTime += dt;
                }
                else
                {
                    movement.stationaryTime = 0;
                }
                
                var direction = movement.movingDirection;
                
                if ((movement.constraints & MovementComponent.Constraints.X) == MovementComponent.Constraints.X)
                {
                    direction.x = 0;
                }
                
                if ((movement.constraints & MovementComponent.Constraints.Y) == MovementComponent.Constraints.Y)
                {
                    direction.y = 0;
                }
                
                if ((movement.constraints & MovementComponent.Constraints.Z) == MovementComponent.Constraints.Z)
                {
                    direction.z = 0;
                }

                var newPosition = position.value;
                Vector3 velocity;
                
                var d = movement.disableNormalizeDirection ? direction : direction.normalized;
                
                if (movement.useAcceleration)
                {
                    if (direction.sqrMagnitude > 0.01f)
                    {
                        // accelerate
                        movement.currentSpeed += movement.acceleration * dt;
                    }
                    else
                    {
                        // deacelerate
                        movement.currentSpeed -= movement.deceleration * dt;
                        // in case of deceleration, I use the current direction
                        d = movement.currentVelocity.normalized;
                    }
                    
                    movement.currentSpeed = Mathf.Clamp(movement.currentSpeed, 0, 
                        movement.speed);
                    velocity = d * movement.currentSpeed * movement.speedMultiplier;
                }
                else
                {
                    movement.currentSpeed = movement.speed;
                    velocity = d * movement.speed * movement.speedMultiplier;
                }

                newPosition += velocity * dt;
                position.value = newPosition;
                movement.currentVelocity = velocity;
            }
        }
    }
}