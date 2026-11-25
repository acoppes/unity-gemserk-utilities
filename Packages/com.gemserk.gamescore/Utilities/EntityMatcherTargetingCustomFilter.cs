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
            return target.entity.Exists() && matcher.Match(target.entity);
        }
    }
}