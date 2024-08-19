using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public class QueryableComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new QueryableComponent());
        }
    }
}