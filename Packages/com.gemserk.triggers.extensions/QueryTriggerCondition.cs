using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public class QueryTriggerCondition : WorldTriggerCondition
    {
        [ObjectType(typeof(Query), disableAssetReferences = true, prefabReferencesOnWhenStart = false)]        
        public Query query;
        
        public override string GetObjectName()
        {
            if (query == null)
            {
                return "MatchQuery()";
            }
            return $"MatchQuery({query.GetEntityQuery().ToString()})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            if (activator == null)
            {
                return false;
            }
            return query.GetEntityQuery().MatchQuery(world, (Entity) activator);
        }
    }
}