using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct SpawnSignalComponent : IEntityComponent
    {
        // spawn data?
    }
    
    public class SpawnSignalComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(SpawnSignalComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new SpawnSignalComponent());
        }
    }
}