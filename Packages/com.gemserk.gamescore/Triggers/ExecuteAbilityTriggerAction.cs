using Game.Components;
using Game.Components.Abilities;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Triggers
{
    public class ExecuteAbilityTriggerAction : WorldTriggerAction
    {
        public enum TargetType
        {
            Fixed = 0,
            Query = 1,
            Target = 2
        }
        
        [DisplayInspector]
        public Query query;

        public string abilityName;

        [FormerlySerializedAs("positionType")] 
        public TargetType targetType = TargetType.Fixed;

        [ConditionalField(nameof(targetType), false, TargetType.Fixed)]
        public Vector3 position;
        
        [ConditionalField(nameof(targetType), false, TargetType.Query)]
        public Query positionQuery;
        
        [ConditionalField(nameof(targetType), false, TargetType.Target)]
        public Query targetQuery;

        public override string GetObjectName()
        {
            if (query == null)
            {
                return $"ExecuteAbility({abilityName})";        
            }
            
            if (targetType == TargetType.Query && positionQuery != null)
            {
                return $"ExecuteAbility({abilityName}, {query}, {positionQuery})";
            }
            
            if (targetType == TargetType.Target && targetQuery != null)
            {
                return $"ExecuteAbility({abilityName}, {query}, {targetQuery})";
            }
            
            return $"ExecuteAbility({abilityName}, {query})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());

            var abilityPosition = this.position;
            Target target = null;
            
            if (targetType == TargetType.Query)
            {
                var e = world.GetFirstOrDefault(positionQuery);
                abilityPosition = e.GetPositionComponent().value;
            }
            
            if (targetType == TargetType.Target)
            {
                var e = world.GetFirstOrDefault(targetQuery);
                abilityPosition = e.GetPositionComponent().value;
                target = e.Get<TargetComponent>().target;
            }
            
            foreach (var entity in entities)
            {
                ref var abilitiesComponent = ref world.GetComponent<AbilitiesComponent>(entity);
                var ability = abilitiesComponent.GetAbility(abilityName);
                ability.pendingExecution = true;
                ability.CopyTarget(new AbilityTarget()
                {
                    position = abilityPosition,
                    valid = true,
                    target = target
                });
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}