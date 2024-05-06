using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gemserk.Triggers.Editor
{
    public class MockTriggerAction : ITrigger.IAction
    {
        public ITrigger.ExecutionResult result;
        public ITrigger.ExecutionResult Execute(object activator = null)
        {
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
    }
}
