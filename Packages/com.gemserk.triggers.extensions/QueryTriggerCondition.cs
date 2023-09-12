using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using MyBox;

namespace Gemserk.Triggers
{
    public class QueryTriggerCondition : TriggerCondition
    {
        [DisplayInspector]
        public Query query;
        
        public override string GetObjectName()
        {
            if (query == null)
            {
                return $"MatchQuery()";
            }
            return $"MatchQuery({query.GetEntityQuery().ToString()})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            var world = World.Default;

            if (activator == null)
            {
                return false;
            }

            return query.GetEntityQuery().MatchQuery(world, (Entity) activator);
        }
    }
}