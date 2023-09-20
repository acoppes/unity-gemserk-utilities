using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct MainPlayerComponent : IEntityComponent
    {
    }
    
    public class MainPlayerComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(MainPlayerComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new MainPlayerComponent());
        }
    }
}