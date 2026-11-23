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
            {
                return false;
            }
            
            return target.IsMatch(world, (Entity)activator, activator);
        }
    }
}