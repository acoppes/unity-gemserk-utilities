using System.Collections.Generic;
using Game.Triggers.Conditions;
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
            Fail = 2
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
            
            return $"Assert.{actionType}()";
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
                    var eval = condition.Evaluate(entity);
                    var assertMessage = $"{condition.GetObjectName()} failed on {entity}";
                    
                    if (!eval)
                    {
                        if (condition is CompareTriggerCondition compareCondition)
                        {
                            var value1 = compareCondition.valueProviderA.GetValue(world, entity);
                            var value2 = compareCondition.valueProviderB.GetValue(world, entity);
                            assertMessage = $"expected {value1}, but was {value2}";
                        }
                    }
                    
                    Assert.IsTrue(eval, assertMessage);
                } 
            }
            else
            {
                var eval = condition.Evaluate(activator);
                var assertMessage = $"{condition.GetObjectName()} failed on {activator}";
                    
                if (!eval)
                {
                    if (condition is CompareTriggerCondition compareCondition)
                    {
                        var value1 = compareCondition.valueProviderA.GetValue(world, activator);
                        var value2 = compareCondition.valueProviderB.GetValue(world, activator);
                        assertMessage = $"expected {value1}, but was {value2}";
                    }
                }
                
                Assert.IsTrue(condition.Evaluate(activator), assertMessage);
            }

            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}