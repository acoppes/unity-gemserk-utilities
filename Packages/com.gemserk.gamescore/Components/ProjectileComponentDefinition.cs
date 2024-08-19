using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;

namespace Game.Components
{
    public struct ProjectileComponent : IEntityComponent
    {
        public enum State
        {
            Pending = 0,
            Traveling,
            Hit
        }

        public enum ProjectileType
        {
            Ranged = 0,
            Melee = 1
        } 
        
        public enum TrajectoryType
        {
            Custom = 0,
            Linear = 1
        } 

        public State state;

        public float travelTime;
        public Vector3 initialVelocity;
        public float initialOffset;

        public Entity source;

        public float travelDistance;
        public float maxDistance;
        public Vector3 previousPosition;

        public ProjectileType projectileType;
        public TrajectoryType trajectoryType;

        public float initialSpeed;

        public bool wasImpacted;
        public bool impacted;

        // events for destination reached, etc

        public Entity impactEntity;
    }

    public struct ProjectileFireComponent : IEntityComponent
    {
        public Vector3 direction;
    }
    
    public class ProjectileComponentDefinition : ComponentDefinitionBase
    {
        public ProjectileComponent.ProjectileType projectileType = ProjectileComponent.ProjectileType.Ranged;
        public ProjectileComponent.TrajectoryType trajectoryType = ProjectileComponent.TrajectoryType.Custom;
        
        public float initialOffset;
        public float maxDistance;

        [ConditionalField(nameof(trajectoryType), false, ProjectileComponent.TrajectoryType.Linear)]
        public float initialSpeed;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ProjectileComponent()
            {
                initialOffset = initialOffset,
                maxDistance = maxDistance,
                projectileType = projectileType,
                trajectoryType = trajectoryType,
                initialSpeed = initialSpeed
            });
        }
    }
}