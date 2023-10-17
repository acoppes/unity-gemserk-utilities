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
    }
}
