using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine.Assertions;

namespace Game.Triggers
{
    public class AssertTriggerAction : WorldTriggerAction
    {
        public TriggerTarget triggerTarget;

        public TriggerCondition condition;
        
        public override string GetObjectName()
        {
            if (condition)
            {
                return $"Assert{condition.GetObjectName()}";
            }
            return "Assert()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            world.GetTriggerTargetEntities(null, triggerTarget, activator, entities);

            if (entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    // if (!condition.Evaluate(entity))
                    // {
                    //     
                    // }
                    Assert.IsTrue(condition.Evaluate(entity), $"{condition.GetObjectName()} failed on {entity}");
                } 
            }
            else
            {
                Assert.IsTrue(condition.Evaluate(activator), $"{condition.GetObjectName()} failed on {activator}");
            }

            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}