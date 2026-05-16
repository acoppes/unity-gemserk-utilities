using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Game.Triggers.Actions
{
    public class PlayerInputTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Switch = 0
        }
        
        public TriggerTarget target;

        public ActionType actionType = ActionType.Switch;

        public int newPlayerInput;

        public override string GetObjectName()
        {
            return $"PlayerInput{actionType}({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            target.Get(entities, world, activator);
            
            foreach (var entity in entities)
            {
                ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
                playerInput.playerInput = newPlayerInput;
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}