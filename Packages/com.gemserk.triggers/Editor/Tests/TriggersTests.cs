using System.Collections;
using Gemserk.Triggers.Actions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gemserk.Triggers.Editor
{
    public class MockTriggerAction : ITrigger.IAction
    {
        public ITrigger.ExecutionResult result;

        public bool disabled;

        public bool Disabled => disabled;
        public int executionTimes;

        public ITrigger.ExecutionResult Execute(object activator = null)
        {
            executionTimes++;
            return result;
        }
    }
    
    public class TriggersTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Trigger_IsRunning_IfActionsAreRunning()
        {
            // Use the Assert class to test conditions

            var trigger = new Trigger();
            trigger.actions.Add(new MockTriggerAction()
            {
                result = ITrigger.ExecutionResult.Running
            });

            trigger.StartExecution();
            
            Assert.AreEqual(ITrigger.ExecutionResult.Running, trigger.Execute());
            Assert.IsTrue(trigger.State == ITrigger.ExecutionState.Executing);
        }
        
        [Test]
        public void Trigger_Completes_IfActionReturnsInterrupt()
        {
            // Use the Assert class to test conditions

            var trigger = new Trigger();
            trigger.actions.Add(new MockTriggerAction()
            {
                result = ITrigger.ExecutionResult.Interrupt
            });
            
            trigger.StartExecution();
            
            Assert.AreEqual(ITrigger.ExecutionResult.Interrupt, trigger.Execute());
            Assert.IsTrue(trigger.State == ITrigger.ExecutionState.Executing);
        }
        
        [Test]
        public void TriggerWith_MaxExecutions_DoNotExecuteAgain()
        {
            // Use the Assert class to test conditions

            var trigger = new Trigger();
            
            trigger.actions.Add(new MockTriggerAction()
            {
                result = ITrigger.ExecutionResult.Completed
            });

            trigger.maxExecutionTimes = 1;
            trigger.QueueExecution();
            Assert.AreEqual(1, trigger.pendingExecutions.Count);
            
            trigger.StartExecution();
            Assert.AreEqual(ITrigger.ExecutionResult.Completed, trigger.Execute());
            trigger.CompleteCurrentExecution();
            
            Assert.AreEqual(1, trigger.executionTimes);
            Assert.IsTrue(trigger.State == ITrigger.ExecutionState.Completed);
            
            trigger.QueueExecution();
            Assert.AreEqual(0, trigger.pendingExecutions.Count);
            Assert.IsTrue(trigger.State == ITrigger.ExecutionState.Completed);
            Assert.AreEqual(1, trigger.executionTimes);
        }
        
        [Test]
        public void Trigger_Completed_CanForceExecutionAgain()
        {
            // Use the Assert class to test conditions

            var trigger = new Trigger();
            
            trigger.actions.Add(new MockTriggerAction()
            {
                result = ITrigger.ExecutionResult.Completed
            });

            trigger.maxExecutionTimes = 1;
            trigger.QueueExecution();
            
            trigger.StartExecution();
            trigger.Execute();
            trigger.CompleteCurrentExecution();
            
            trigger.ForceQueueExecution();
            Assert.AreEqual(1, trigger.pendingExecutions.Count);
            Assert.IsTrue(trigger.State == ITrigger.ExecutionState.PendingExecution);
            
            trigger.StartExecution();
            trigger.Execute();
            trigger.CompleteCurrentExecution();
            
            Assert.IsTrue(trigger.State == ITrigger.ExecutionState.Completed);
            Assert.AreEqual(2, trigger.executionTimes);
        }
        
        [Test]
        public void DisabledAction_IsNotExecuted()
        {
            // Use the Assert class to test conditions

            var trigger = new Trigger();

            var mockAction = new MockTriggerAction()
            {
                disabled = true,
                result = ITrigger.ExecutionResult.Running
            };
            
            trigger.actions.Add(mockAction);

            trigger.QueueExecution();
            trigger.StartExecution();
            trigger.Execute();
            trigger.CompleteCurrentExecution();
            
            Assert.AreEqual(1, trigger.executionTimes);
            Assert.AreEqual(0, mockAction.executionTimes);
        }
        
        [Test]
        public void ClearPendingExecutions()
        {
            var trigger = new Trigger();
            
            trigger.actions.Add(new MockTriggerAction()
            {
                result = ITrigger.ExecutionResult.Completed
            });

            trigger.QueueExecution();
            trigger.QueueExecution();

            Assert.AreEqual(2, trigger.pendingExecutions.Count);
            
            trigger.ClearPendingExecutions();
            
            Assert.AreEqual(0, trigger.pendingExecutions.Count);
        }
        
        [Test]
        public void ClearPendingExecutions_WithTrigger()
        {
            var gameObject = new GameObject();
            
            var actions = new GameObject("Actions");
            actions.transform.SetParent(gameObject.transform);
            
            var action1 = new GameObject("Action1");
            action1.transform.SetParent(actions.transform);
            
            var clearPending = action1.gameObject.AddComponent<ClearTriggerExecutionsTriggerAction>();
            
            var triggerObject = gameObject.AddComponent<TriggerObject>();
            triggerObject.Awake();
            
            clearPending.trigger = triggerObject;

            triggerObject.QueueExecution();
            triggerObject.QueueExecution();
            
            Assert.AreEqual(2, triggerObject.trigger.pendingExecutions.Count);
            
            triggerObject.StartExecution();
            triggerObject.Execute();
            triggerObject.CompleteCurrentExecution();
            
            Assert.AreEqual(0, triggerObject.trigger.pendingExecutions.Count);
        }
        
        [Test]
        public void DontExecute_IfNotEnabled()
        {
            var gameObject = new GameObject();
            
            var actions = new GameObject("Actions");
            actions.transform.SetParent(gameObject.transform);
            
            var action1 = new GameObject("Action1");
            action1.transform.SetParent(actions.transform);
            
            var triggerObject = gameObject.AddComponent<TriggerObject>();
            triggerObject.Awake();

            // var triggerSystem = new TriggerSystemExecutor();
            // triggerSystem.triggers.Add(triggerObject);
            
            triggerObject.trigger.SetEnabled(false);

            triggerObject.QueueExecution();
            triggerObject.QueueExecution();
            
            Assert.AreEqual(0, triggerObject.trigger.pendingExecutions.Count);
            triggerObject.trigger.SetEnabled(true);
            
            triggerObject.QueueExecution();
            triggerObject.QueueExecution();
            
            Assert.AreEqual(2, triggerObject.trigger.pendingExecutions.Count);
            
            triggerObject.trigger.SetEnabled(false);
            Assert.AreEqual(0, triggerObject.trigger.pendingExecutions.Count);
            
            // triggerSystem.Execute();
        }
        
        [Test]
        public void TriggerObject_DelegateEnableToTrigger()
        {
            var gameObject = new GameObject();
            
            var triggerObject = gameObject.AddComponent<TriggerObject>();
            triggerObject.Awake();

            Assert.IsFalse(triggerObject.IsDisabled());
            gameObject.SetActive(false);
            triggerObject.OnDisable();
            Assert.IsTrue(triggerObject.IsDisabled());
            gameObject.SetActive(true);
            triggerObject.OnEnable();
            Assert.IsFalse(triggerObject.IsDisabled());
        }
    }
}
