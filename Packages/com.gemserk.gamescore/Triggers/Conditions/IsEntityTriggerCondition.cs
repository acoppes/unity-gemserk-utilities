using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

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
                return false;
            
            var results = new List<Entity>();
            target.Get(results, world, activator);
            return results.Contains((Entity)activator);
        }
    }
}