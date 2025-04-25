using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public class NotQueryParameter : QueryParameterBase
    {
        public QueryParameterBase queryParameter;
        
        public override bool MatchQuery(Entity entity)
        {
            return !queryParameter.MatchQuery(entity);
        }
    }
}