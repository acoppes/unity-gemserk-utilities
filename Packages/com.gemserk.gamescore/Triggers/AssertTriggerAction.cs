using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using MyBox;
using UnityEngine.Assertions;

namespace Game.Triggers
{
    public class AssertTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            MatchCondition = 0,
            Pass = 1,
            Fail = 0
        }

        public ActionType actionType = ActionType.MatchCondition;
        
        [ConditionalField(nameof(actionType), false, ActionType.MatchCondition)]
        public TriggerTarget triggerTarget;

        [ConditionalField(nameof(actionType), false, ActionType.MatchCondition)]
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
            if (actionType == ActionType.Pass)
            {
                // TODO: ideally here there will be some static field/method to check.
#if UNITY_EDITOR
                // if not running a test runner, then auto stop unity editor.
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return ITrigger.ExecutionResult.Interrupt;
            }
            
            if (actionType == ActionType.Fail)
            {
                // TODO: ideally here there will be some static field/method to check.
#if UNITY_EDITOR
                // if not running a test runner, then auto stop unity editor.
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return ITrigger.ExecutionResult.Interrupt;
            }
            
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