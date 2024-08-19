using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct CameraComponent : IEntityComponent
    {
        
    }
    
    public class CameraComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CameraComponent());
        }
    }
}