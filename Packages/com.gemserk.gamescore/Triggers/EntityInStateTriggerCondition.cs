using System;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;

namespace Game.Triggers
{
    public class EntityInStateTriggerCondition : WorldTriggerCondition
    {
        public Query query;
        public string state;
        
        public override string GetObjectName()
        {
            if (query == null)
            {
                return $"InState({state})";
            }
            return $"InState({state}, {query})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            if (query == null || world == null)
            {
                return false;
            }

            var entities = world.GetEntities(query.GetEntityQuery());

            var hasState = true;

            foreach (var entity in entities)
            {
                if (!entity.Has<StatesComponent>())
                {
                    throw new Exception("Invalid entity without states component");
                }

                hasState = hasState && entity.GetStatesComponent().HasState(state);
            }

            return hasState;
        }
    }
}