using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct LimitMaxFallingVelocityComponent : IEntityComponent
    {
        
    }
    
    public class LimitMaxFallingVelocityComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(LimitMaxFallingVelocityComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new LimitMaxFallingVelocityComponent());
        }
    }
}