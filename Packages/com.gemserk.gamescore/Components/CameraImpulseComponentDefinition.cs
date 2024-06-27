using Gemserk.Leopotam.Ecs;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Components
{
    public struct CameraImpulseComponent : IEntityComponent
    {
        public enum DirectionSource
        {
            FromImpulseSource = 0,
            FromLookingDirection = 1,
            Random = 2
        }
        
        public float force;
        public Vector3 direction;
        public DirectionSource directionSource;
        public CinemachineImpulseSource impulseSource;

        public int framesToGenerateImpulse;
        public int framesToRemove;
    }
    
    public class CameraImpulseComponentDefinition : ComponentDefinitionBase
    {
        public float force;
        public CameraImpulseComponent.DirectionSource directionSource = CameraImpulseComponent.DirectionSource.FromImpulseSource;
        public CinemachineImpulseSource impulseSource;

        public int framesToRemove = 1;
        public int framesToGenerateImpulse = 1;
        
        public override string GetComponentName()
        {
            return nameof(CameraImpulseComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CameraImpulseComponent()
            {
                force = force,
                impulseSource = impulseSource,
                framesToRemove = framesToRemove,
                directionSource = directionSource,
                framesToGenerateImpulse = framesToGenerateImpulse
            });
        }
    }
}