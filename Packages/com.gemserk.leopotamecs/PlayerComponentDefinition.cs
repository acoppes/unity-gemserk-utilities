namespace Gemserk.Leopotam.Ecs
{
    public class PlayerComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(PlayerComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PlayerComponent());
        }
    }
}