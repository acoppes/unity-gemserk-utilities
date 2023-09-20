using Game.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class ControlTriggerAction : WorldTriggerAction
    {
        [DisplayInspector]
        public Query query;

        public Vector2 direction;
        
        public bool left;
        public bool right;
        
        public bool button1;
        public bool button2;
        public bool button3;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());
            
            foreach (var entity in entities)
            {
                ref var control = ref world.GetComponent<InputComponent>(entity);
                control.direction().vector2 = direction;
                
                control.left().UpdatePressed(left);
                control.right().UpdatePressed(right);
            
                control.button1().UpdatePressed(button1);
                control.button2().UpdatePressed(button2);

                if (control.IsActionDefined(nameof(button3)))
                {
                    control.actions[nameof(button3)].UpdatePressed(button3);
                    // control.button3.UpdatePressed(button3);
                }
               
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}