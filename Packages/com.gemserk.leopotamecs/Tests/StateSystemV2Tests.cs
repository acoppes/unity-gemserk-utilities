using System;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using MyGame;
using NUnit.Framework;

namespace Gemserk.Leopotam.Ecs.Tests
{
    public class StateSystemV2Tests
    {
        public class StatesHandlerMock
        {
            public int onEnterCalls;
            public int onExitCalls;

            public void OnStatesEnter(StatesComponentV2 states)
            {
                onEnterCalls++;
            }

            public void OnStatesExit(StatesComponentV2 states)
            {
                onExitCalls++;
            }
        }
        
        public class StatesHandleExitFirstMock
        {
            public bool exitCalled;

            public void OnStatesEnter(StatesComponentV2 states)
            {
                if (exitCalled)
                    return;
                
                throw new Exception("Exit must be called first");
            }

            public void OnStatesExit(StatesComponentV2 states)
            {
                exitCalled = true;
            }
        }
        
        [Test]
        public void State_EnterPending_AfterEnterState()
        {
            var statesComponent = StatesComponentV2.Create();
            
            statesComponent.Enter(0);
            
            Assert.IsTrue(statesComponent.HasState(0));
            Assert.IsFalse(statesComponent.HasEnteredInLastFrame(0));
            
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            
            Assert.IsTrue(statesComponent.HasEnteredInLastFrame(0));
            
            statesComponent.Exit(0);
            
            Assert.IsFalse(statesComponent.HasState(0));
            Assert.IsFalse(statesComponent.HasExitLastFrame(0));

            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            
            Assert.IsTrue(statesComponent.HasExitLastFrame(0));
        }
        
        [Test]
        public void State_Callbacks_Tests()
        {
            var statesComponent = StatesComponentV2.Create();
            var statesHandler = new StatesHandlerMock();
            
            statesComponent.Enter(0);
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
        
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            StatesTransitionsSystemV2.InvokeStatesCallbacks(ref statesComponent);
        
            Assert.AreEqual(1, statesHandler.onEnterCalls);
            Assert.AreEqual(0, statesHandler.onExitCalls);
            
            statesComponent.Exit(0);
            
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            StatesTransitionsSystemV2.InvokeStatesCallbacks(ref statesComponent);
            
            Assert.AreEqual(1, statesHandler.onEnterCalls);
            Assert.AreEqual(1, statesHandler.onExitCalls);
        }
        
        //
        // [Test]
        // public void ExitStates_Before_EnterStates()
        // {
        //     var statesComponent = StatesComponent.Create();
        //     var statesHandler = new StatesHandleExitFirstMock();
        //     
        //     statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
        //     statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
        //     
        //     statesComponent.EnterState("A");
        //     
        //     StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
        //     
        //     statesComponent.ExitState("A");
        //     statesComponent.EnterState("B");
        //     
        //     StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
        //     StatesTransitionsSystem.InvokeStatesCallbacks(statesComponent);
        // }
        //
        // [Test]
        // public void Test_DebugStates()
        // {
        //     var statesComponent = StatesComponent.Create();
        //     statesComponent.debugTransitions = true;
        //     
        //     statesComponent.EnterState("A");
        //     
        //     StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
        //     
        //     statesComponent.EnterState("B");
        //     statesComponent.ExitState("A");
        //     
        //     StatesTransitionsSystem.UpdateStatesTransitions(statesComponent);
        // }
        //
        // [Test]
        // public void AutoExit_OnDurationCompleted()
        // {
        //     var statesComponent = StatesComponent.Create();
        //     var statesHandler = new StatesHandlerMock();
        //     
        //     statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
        //     statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
        //     
        //     statesComponent.EnterState("A");
        //     
        //     StatesSystem.UpdateStateTime(statesComponent, 0.5f);
        //     
        //     Assert.AreEqual(0.5f, statesComponent.GetState("A").time);
        //     Assert.IsTrue(statesComponent.HasState("A"));
        //     
        //     statesComponent.EnterState("B", 0.1f);
        //     
        //     StatesSystem.UpdateStateTime(statesComponent, 0.2f);
        //     
        //     Assert.IsFalse(statesComponent.HasState("B"));
        // }
        //
        // [Test]
        // public void ExitState_AfterTime()
        // {
        //     var statesComponent = StatesComponent.Create();
        //     var statesHandler = new StatesHandlerMock();
        //     
        //     statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
        //     statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
        //     
        //     statesComponent.EnterState("A");
        //     
        //     StatesSystem.UpdateStateTime(statesComponent, 0.5f);
        //     
        //     Assert.AreEqual(0.5f, statesComponent.GetState("A").time);
        //     Assert.IsTrue(statesComponent.HasState("A"));
        //     
        //     statesComponent.ExitState("A", 0.2f);
        //     
        //     StatesSystem.UpdateStateTime(statesComponent, 0.1f);
        //     
        //     Assert.AreEqual(true, statesComponent.HasState("A"));
        //     
        //     StatesSystem.UpdateStateTime(statesComponent, 0.1f);
        //     
        //     Assert.AreEqual(false, statesComponent.HasState("A"));
        // }
    }
}