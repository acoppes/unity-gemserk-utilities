namespace Gemserk.Leopotam.Ecs
{
    public class PlayerComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public int startingPlayer;
        
        public override string GetComponentName()
        {
            return nameof(PlayerComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            if (!world.HasComponent<PlayerComponent>(entity))
            {
                world.AddComponent(entity, new PlayerComponent());
            }
            entity.Get<PlayerComponent>().player = startingPlayer;
        }
    }
}