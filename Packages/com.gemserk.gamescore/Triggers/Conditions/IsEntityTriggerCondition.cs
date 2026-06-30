using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine.Pool;

namespace Game.Triggers.Conditions
{
    public class IsEntityTriggerCondition : WorldTriggerCondition
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
            return $"IsEntity({target})";
        }

        public override bool Evaluate(object activator = null)
        {
            if (activator == null)
            {
                return false;
            }
            
            var results = ListPool<Entity>.Get();
            var isEntity = false;
            
            if (target.Get(results, world, activator))
            {
                isEntity = results.Contains((Entity)activator);
            }
            
            ListPool<Entity>.Release(results);
            return isEntity;
        }
    }
}