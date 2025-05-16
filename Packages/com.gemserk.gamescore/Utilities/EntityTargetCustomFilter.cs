using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Utilities
{
    public class EntityTargetCustomFilter : MonoBehaviour, ITargetCustomFilter
    {
        public QueryParameterBase matcher;
        
        public bool Filter(Target target, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            if (!target.entity.Exists())
                return false;
            
            return matcher.MatchQuery(target.entity);
        }
    }
}