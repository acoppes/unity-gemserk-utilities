using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct PhysicsMovementComponent : IEntityComponent
    {
        
    }
    
    public class PhysicsMovementComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PhysicsMovementComponent());
        }
    }
}