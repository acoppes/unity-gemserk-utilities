using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class QueryTriggerCondition : WorldTriggerCondition
    {
        [ObjectType(typeof(Query), disableAssetReferences = true, prefabReferencesOnWhenStart = false)]        
        public Object query;
        
        public override string GetObjectName()
        {
            if (query == null)
            {
                return "MatchQuery()";
            }
            return $"MatchQuery({query.GetInterface<Query>().GetEntityQuery().ToString()})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            if (activator == null)
            {
                return false;
            }
            return query.GetInterface<Query>().MatchQuery(world, (Entity) activator);
        }
    }
}