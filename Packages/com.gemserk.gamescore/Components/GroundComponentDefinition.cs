using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public class GroundComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(GroundComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new GroundComponent());
        }
    }
}