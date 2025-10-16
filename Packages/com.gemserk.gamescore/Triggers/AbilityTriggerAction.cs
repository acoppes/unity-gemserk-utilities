using System.Collections.Generic;
using Game.Components;
using Game.Components.Abilities;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class AbilityTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Execute = 0,
            LoadCooldown = 1
        }
        
        public enum TargetType
        {
            Fixed = 0,
            Position = 1,
            Target = 2,
            None = 3
        }

        public ActionType actionType = ActionType.Execute;
        
        public TriggerTarget actor;
        
        public string abilityName;

        [ConditionalField(nameof(actionType), false, ActionType.Execute)]
        public TargetType targetType = TargetType.None;

        [ConditionalField(nameof(targetType), false, TargetType.Fixed)]
        public Vector3 position;
        
        [ConditionalField(nameof(targetType), false, TargetType.Position)]
        public TriggerTarget positionTarget;
        
        [ConditionalField(nameof(targetType), false, TargetType.Target)]
        public TriggerTarget targetTarget;

        public override string GetObjectName()
        {
            if (targetType == TargetType.Position)
            {
                return $"Ability_{actionType}({abilityName}, {actor}, {positionTarget})";
            }
            
            if (targetType == TargetType.Target)
            {
                return $"Ability_{actionType}({abilityName}, {actor}, {targetTarget})";
            }
            
            return $"Ability_{actionType}({abilityName}, {actor}, {position})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            world.GetTriggerTargetEntities(null, actor, activator, entities);

            var abilityPosition = this.position;
            Target target = null;
            
            if (targetType == TargetType.Position)
            {
                var e = world.GetTriggerFirstEntity(null, positionTarget, activator);
                abilityPosition = e.GetPositionComponent().value;
            }
            
            if (targetType == TargetType.Target)
            {
                var e = world.GetTriggerFirstEntity(null, targetTarget, activator);
                abilityPosition = e.GetPositionComponent().value;
                target = e.Get<TargetComponent>().target;
            }
            
            foreach (var entity in entities)
            {
                ref var abilitiesComponent = ref world.GetComponent<AbilitiesComponent>(entity);
                var ability = abilitiesComponent.GetAbility(abilityName);
                
                if (actionType == ActionType.Execute)
                {
                    ability.pendingExecution = true;
                    ability.CopyTarget(new AbilityTarget()
                    {
                        position = abilityPosition,
                        valid = true,
                        target = target
                    });
                } else if (actionType == ActionType.LoadCooldown)
                {
                    ability.cooldown.Fill();
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}