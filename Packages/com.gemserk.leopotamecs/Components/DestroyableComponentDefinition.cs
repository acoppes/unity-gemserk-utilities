namespace Gemserk.Leopotam.Ecs.Components
{
    public struct DestroyableComponent : IEntityComponent
    {
        public bool destroy;
    }
    
    public class DestroyableComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(DestroyableComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DestroyableComponent());
        }
    }
}