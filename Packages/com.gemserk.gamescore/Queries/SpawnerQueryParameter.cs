using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public struct SpawnerParameter : IEntityMatcher
    {
        public bool Match(Entity entity)
        {
            return entity.Has<SpawnerComponent>();
        }
    }
    
    public class SpawnerQueryParameter : EntityMatcherBase
    {
        public override bool Match(Entity entity)
        {
            return new SpawnerParameter().Match(entity);
        }

        public override string ToString()
        {
            return "isSpawner";
        }
    }
}