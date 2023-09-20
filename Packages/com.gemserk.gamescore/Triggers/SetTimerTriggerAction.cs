using Game.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using MyBox;

namespace Game.Triggers
{
    public class SetTimerTriggerAction : WorldTriggerAction
    {
        [DisplayInspector]
        public Query query;

        public float time;
        
        public override string GetObjectName()
        {
            if (query != null)
            {
                return $"SetTimer({time}, {query})";
            }

            return $"SetTimer({time})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());
            
            foreach (var entity in entities)
            {
                ref var timerComponent = ref world.GetComponent<TimerComponent>(entity);
                timerComponent.timer = new Cooldown(time);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}