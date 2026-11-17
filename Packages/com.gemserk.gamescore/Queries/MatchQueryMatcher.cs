using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public readonly struct MatchQueryEntityMatcher : IEntityMatcher
    {
        private readonly EntityQuery query;
        
        public MatchQueryEntityMatcher(EntityQuery query)
        {
            this.query = query;
        }
        
        public bool Match(Entity entity)
        {
            return query.MatchQuery(entity);
        }
    }
    
    public class MatchQueryMatcher : EntityMatcherBase
    {
        [Query]
        public Query query;

        public override bool Match(Entity entity)
        {
            return new MatchQueryEntityMatcher(query.GetEntityQuery()).Match(entity);
        }

        public override string ToString()
        {
            if (query)
                return $"matchQuery({query.name})";
            return "matchQuery()";
        }
    }
}