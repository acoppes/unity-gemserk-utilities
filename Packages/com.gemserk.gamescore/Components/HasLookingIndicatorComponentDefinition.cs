using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public class HasLookingIndicatorComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(HasLookingDirectionIndicatorComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new HasLookingDirectionIndicatorComponent());
        }
    }
}