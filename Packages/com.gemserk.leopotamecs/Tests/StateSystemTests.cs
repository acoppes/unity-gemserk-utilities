using System;
using Gemserk.Leopotam.Ecs.Controllers;
using NUnit.Framework;

namespace Gemserk.Leopotam.Ecs.Tests
{
    public class StateSystemTests
    {
        public class StatesHandlerMock
        {
            public int onEnterCalls;
            public int onExitCalls;

            public void OnStatesEnter(StatesComponent states)
            {
                onEnterCalls++;
            }

            public void OnStatesExit(StatesComponent states)
            {
                onExitCalls++;
            }
        }
        
        public class StatesHandleExitFirstMock
        {
            public bool exitCalled;

            public void OnStatesEnter(StatesComponent states)
            {
                if (exitCalled)
                    return;
                
                throw new Exception("Exit must be called first");
            }

            public void OnStatesExit(StatesComponent states)
            {
                exitCalled = true;
            }
        }
        
        [Test]
        public void State_EnterPending_AfterEnterState()
        {
            var statesComponent = StatesComponent.Create();
            
            statesComponent.EnterState("A");
            
            Assert.IsTrue(statesComponent.HasState("A"));
            Assert.IsFalse(statesComponent.statesEntered.Contains("A"));
            
            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            
            Assert.IsTrue(statesComponent.statesEntered.Contains("A"));
            
            statesComponent.ExitState("A");
            
            Assert.IsFalse(statesComponent.HasState("A"));
            Assert.IsFalse(statesComponent.statesExited.Contains("A"));

            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            
            Assert.IsTrue(statesComponent.statesExited.Contains("A"));
        }
        
        [Test]
        public void State_Callbacks_Tests()
        {
            var statesComponent = StatesComponent.Create();
            var statesHandler = new StatesHandlerMock();
            
            statesComponent.EnterState("A");
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;

            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            StatesTransitionsSystem.InvokeStatesCallbacks(statesComponent);

            Assert.AreEqual(1, statesHandler.onEnterCalls);
            Assert.AreEqual(0, statesHandler.onExitCalls);
            
            statesComponent.ExitState("A");
            
            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            StatesTransitionsSystem.InvokeStatesCallbacks(statesComponent);
            
            Assert.AreEqual(1, statesHandler.onEnterCalls);
            Assert.AreEqual(1, statesHandler.onExitCalls);
        }
        
        [Test]
        public void ExitStates_Before_EnterStates()
        {
            var statesComponent = StatesComponent.Create();
            var statesHandler = new StatesHandleExitFirstMock();
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
            
            statesComponent.EnterState("A");
            
            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            
            statesComponent.ExitState("A");
            statesComponent.EnterState("B");
            
            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            StatesTransitionsSystem.InvokeStatesCallbacks(statesComponent);
        }
        
        [Test]
        public void Test_DebugStates()
        {
            var statesComponent = StatesComponent.Create();
            statesComponent.debugTransitions = true;
            
            statesComponent.EnterState("A");
            
            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
            
            statesComponent.EnterState("B");
            statesComponent.ExitState("A");
            
            StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
        }
        
        [Test]
        public void AutoExit_OnDurationCompleted()
        {
            var statesComponent = StatesComponent.Create();
            var statesHandler = new StatesHandlerMock();
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
            
            statesComponent.EnterState("A");
            
            StatesSystem.UpdateStateTime(statesComponent, 0.5f);
            
            Assert.AreEqual(0.5f, statesComponent.GetState("A").time);
            Assert.IsTrue(statesComponent.HasState("A"));
            
            statesComponent.EnterState("B", 0.1f);
            
            StatesSystem.UpdateStateTime(statesComponent, 0.2f);
            
            Assert.IsFalse(statesComponent.HasState("B"));
        }
    }
}