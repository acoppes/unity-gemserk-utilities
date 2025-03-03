using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Game.Triggers.Actions
{
    public class PlayerTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            SwitchTeam = 0
        }
        
        public TriggerTarget target;

        public ActionType actionType = ActionType.SwitchTeam;

        public int newPlayerTeam;

        public override string GetObjectName()
        {
            return $"Player{actionType}({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            target.Get(entities, world, activator);
            
            foreach (var entity in entities)
            {
                ref var player = ref world.GetComponent<PlayerComponent>(entity);
                player.player = newPlayerTeam;
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}