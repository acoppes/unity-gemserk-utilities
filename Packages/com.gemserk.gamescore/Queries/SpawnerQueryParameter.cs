using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public struct SpawnerParameter : IQueryParameter
    {
        public bool MatchQuery(Entity entity)
        {
            return entity.Has<SpawnerComponent>();
        }
    }
    
    public class SpawnerQueryParameter : QueryParameterBase
    {
        public override bool MatchQuery(Entity entity)
        {
            return new SpawnerParameter().MatchQuery(entity);
        }

        public override string ToString()
        {
            return "isSpawner";
        }
    }
}