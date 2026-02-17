using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers; 
using Gemserk.Utilities;

namespace Game.Triggers
{
    public class SetTimerTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        public float time;
        
        public override string GetObjectName()
        {
            return $"SetTimer({time}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();

            if (target.Get(entities, world, activator))
            {
                foreach (var entity in entities)
                {
                    ref var timerComponent = ref world.GetComponent<TimerComponent>(entity);
                    timerComponent.timer = new Cooldown(time);
                    timerComponent.paused = false;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}