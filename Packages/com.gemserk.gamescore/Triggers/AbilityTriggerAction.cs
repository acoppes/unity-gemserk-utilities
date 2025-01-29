using System.Collections.Generic;
using Game.Components;
using Game.Components.Abilities;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Game.Triggers
{
    public class AbilityTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            LoadCooldown = 0
        }
        
        public ActionType actionType = ActionType.LoadCooldown;
        
        public TriggerTarget target;
        public string abilityName;

        public override string GetObjectName()
        {
            return $"{actionType}Ability({abilityName}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            target.Get(entities, world, activator);
            
            foreach (var entity in entities)
            {
                ref var abilitiesComponent = ref world.GetComponent<AbilitiesComponent>(entity);
                var ability = abilitiesComponent.GetAbility(abilityName);
                ability.cooldown.Fill();
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}