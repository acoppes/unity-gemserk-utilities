using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;

namespace Game.Triggers
{
    public class EntityStateTransitionTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Enter = 0,
            Exit = 1
        }

        public ActionType actionType;
        
        // optional duration for the states
        public float duration;

        public Query entityQuery;
        public List<string> states = new List<string>();

        public override string GetObjectName()
        {
            return $"{actionType}({string.Join(",", states)})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(entityQuery);

            if (actionType == ActionType.Enter)
            {
                foreach (var entity in entities)
                {
                    if (entity.Has<StatesComponent>())
                    {
                        ref var statesComponent = ref entity.GetStatesComponent();
                        statesComponent.EnterStates(states, duration);
                    }
                }
            }
            
            if (actionType == ActionType.Exit)
            {
                foreach (var entity in entities)
                {
                    if (entity.Has<StatesComponent>())
                    {
                        ref var statesComponent = ref entity.GetStatesComponent();
                        statesComponent.ExitStates(states, duration);
                    }
                }
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}