using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class ControllableTriggerAction : WorldTriggerAction
    {
        [DisplayInspector]
        public Query query;

        public ControllableByComponent.ControllableType controllableType;

        public override string GetObjectName()
        {
            if (query != null)
            {
                var queryString = query.ToString();
                return $"SetControllable({queryString})";
            }
            
            return "SetControllable()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());
            
            foreach (var entity in entities)
            {
                ref var controllable = ref world.GetComponent<ControllableByComponent>(entity);
                controllable.controllableType = controllableType;
                
                ref var control = ref world.GetComponent<InputComponent>(entity);
                if (control.IsActionDefined("movement"))
                {
                    control.direction().vector2 = Vector2.zero;
                }
                
                entity.Get<BufferedInputComponent>().ConsumeBuffer();
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}