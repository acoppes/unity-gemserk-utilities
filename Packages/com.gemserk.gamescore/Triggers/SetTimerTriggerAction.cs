using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers; 
using Gemserk.Utilities;
using MyBox;

namespace Game.Triggers
{
    public class SetTimerTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Set = 0,
            Reset = 1
        }

        public ActionType actionType = ActionType.Set;
        
        public TriggerTarget target;
        
        [ConditionalField(nameof(actionType),false, ActionType.Set)]
        public float time;
        
        public override string GetObjectName()
        {
            if (actionType == ActionType.Set)
            {
                return $"Timer{actionType}({time}, {target})";
            }
            
            return $"Timer{actionType}({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();

            if (target.Get(entities, world, activator))
            {
                foreach (var entity in entities)
                {
                    ref var timerComponent = ref world.GetComponent<TimerComponent>(entity);
                    
                    if (actionType == ActionType.Set)
                    {
                        timerComponent.timer = new Cooldown(time);
                        timerComponent.paused = false;
                    } else if (actionType == ActionType.Reset)
                    {
                        timerComponent.timer.Reset();
                    }
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}