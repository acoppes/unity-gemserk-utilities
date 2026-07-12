using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Utilities
{
    public class EntityMatcherTargetingCustomFilter : MonoBehaviour, ITargetCustomFilter
    {
        public EntityMatcherBase matcher;
        
        public bool Filter(Target target, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            if (!target.entity.Exists())
                return false;
            
            return matcher.Match(target.entity);
        }
    }
}