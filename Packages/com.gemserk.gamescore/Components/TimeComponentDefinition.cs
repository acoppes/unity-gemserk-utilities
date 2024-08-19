using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct TimeComponent : IEntityComponent
    {
        public float time;
    }
    
    public class TimeComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new TimeComponent());
        }
    }
}