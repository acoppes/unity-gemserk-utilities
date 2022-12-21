using Gemserk.Leopotam.Gameplay.Controllers;
using NUnit.Framework;

namespace Gemserk.Leopotam.Gameplay.Tests
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
        
        [Test]
        public void State_EnterPending_AfterEnterState()
        {
            var statesComponent = StatesComponent.Create();
            
            statesComponent.EnterState("A");
            
            Assert.IsTrue(statesComponent.HasState("A"));
            Assert.IsFalse(statesComponent.statesEntered.Contains("A"));
            
            StatesSystem.UpdateStatesTransitions(statesComponent);
            
            Assert.IsTrue(statesComponent.statesEntered.Contains("A"));
            
            statesComponent.ExitState("A");
            
            Assert.IsFalse(statesComponent.HasState("A"));
            Assert.IsFalse(statesComponent.statesExited.Contains("A"));

            StatesSystem.UpdateStatesTransitions(statesComponent);
            
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

            StatesSystem.UpdateStatesTransitions(statesComponent);
            StatesSystem.InvokeStatesCallbacks(statesComponent);

            Assert.AreEqual(1, statesHandler.onEnterCalls);
            Assert.AreEqual(0, statesHandler.onExitCalls);
            
            statesComponent.ExitState("A");
            
            StatesSystem.UpdateStatesTransitions(statesComponent);
            StatesSystem.InvokeStatesCallbacks(statesComponent);
            
            Assert.AreEqual(1, statesHandler.onEnterCalls);
            Assert.AreEqual(1, statesHandler.onExitCalls);
        }
    }
}