using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    // just to declare the entity can be named
    public class NameComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(NameComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new NameComponent());
        }
    }
}