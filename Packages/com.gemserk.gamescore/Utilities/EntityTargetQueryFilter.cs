using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Utilities
{
    public class EntityTargetQueryFilter : MonoBehaviour, ITargetCustomFilter
    {
        public Query matcher;
        
        public bool Filter(Target target, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            if (!target.entity.Exists())
                return false;

            return matcher.GetEntityQuery().MatchQuery(target.entity);
        }
    }
}