using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class ControllableTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;

        public ControllableByComponent.ControllableType controllableType;

        private readonly List<Entity> entities = new List<Entity>();
        
        public override string GetObjectName()
        {
            return $"SetControllable({target}, {controllableType})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            entities.Clear();
            target.Get(entities, world, activator);
            
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
            
            entities.Clear();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}