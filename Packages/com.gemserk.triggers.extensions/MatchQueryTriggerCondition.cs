using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class MatchQueryTriggerCondition : WorldTriggerCondition
    {
        [Query]
        public Object query;
        
        public override string GetObjectName()
        {
            if (!query)
            {
                return "MatchQuery()";
            }
            return $"MatchQuery({query.GetInterface<Query>().name})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            if (activator == null)
            {
                return false;
            }

            var isMatch = query.GetInterface<Query>().MatchQuery(world, (Entity) activator);
            return isMatch;
        }
    }
}