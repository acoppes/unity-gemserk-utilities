using Gemserk.Leopotam.Ecs;
using Unity.Cinemachine;

namespace Game.Components
{
    public struct CameraImpulseComponent : IEntityComponent
    {
        public float force;
        public CinemachineImpulseSource impulseSource;
    }
    
    public class CameraImpulseComponentDefinition : ComponentDefinitionBase
    {
        public float force;
        public CinemachineImpulseSource impulseSource;
        
        public override string GetComponentName()
        {
            return nameof(CameraImpulseComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CameraImpulseComponent()
            {
                force = force,
                impulseSource = impulseSource
            });
        }
    }
}