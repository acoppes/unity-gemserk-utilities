using System;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using MyGame;
using NUnit.Framework;
using UnityEngine;

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
        
        
        [Test]
        public void ExitStates_Before_EnterStates()
        {
            var statesComponent = StatesComponentV2.Create();
            var statesHandler = new StatesHandleExitFirstMock();
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
            
            statesComponent.Enter(0);
            
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            
            statesComponent.Exit(0);
            statesComponent.Enter(0);
            
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            StatesTransitionsSystemV2.InvokeStatesCallbacks(ref statesComponent);
        }
        
        
        [Test]
        public void Test_DebugStates()
        {
            var statesComponent = StatesComponentV2.Create();
            statesComponent.debugTransitions = true;

            var state = 0;
            var stateB = 1;
            
            statesComponent.Enter(state);
            
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
            
            statesComponent.Enter(stateB);
            statesComponent.Exit(state);
            
            StatesTransitionsSystemV2.UpdateStatesTransitions(ref statesComponent);
        }
        
        [Test]
        public void AutoExit_OnDurationCompleted()
        {
            var statesComponent = StatesComponentV2.Create();
            var statesHandler = new StatesHandlerMock();
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;
            
            statesComponent.Enter(0);
            
            StatesSystemV2.UpdateStateTime(ref statesComponent, 0.5f);
            
            Assert.IsTrue(statesComponent.HasState(0));
            Assert.AreEqual(0.5f, statesComponent.GetState(0).time);
            
            statesComponent.Enter(1, 0.1f);
            
            Assert.IsTrue(statesComponent.HasState(1));
            Assert.AreEqual(0.1f, statesComponent.GetState(1).duration, 0.01f);
            
            StatesSystemV2.UpdateStateTime(ref statesComponent, 0.2f);
            
            Assert.IsFalse(statesComponent.HasState(1));
        }
        
        [Test]
        public void ExitState_AfterTime()
        {
            var statesComponent = StatesComponentV2.Create();
            var statesHandler = new StatesHandlerMock();
            
            statesComponent.onStatesEnterEvent += statesHandler.OnStatesEnter;
            statesComponent.onStatesExitEvent += statesHandler.OnStatesExit;

            var state = 0;
            
            statesComponent.Enter(state);
            
            StatesSystemV2.UpdateStateTime(ref statesComponent, 0.5f);
            
            Assert.AreEqual(0.5f, statesComponent.GetState(state).time);
            Assert.IsTrue(statesComponent.HasState(state));
            
            statesComponent.Exit(state, 0.2f);
            
            StatesSystemV2.UpdateStateTime(ref statesComponent, 0.1f);
            
            Assert.AreEqual(true, statesComponent.HasState(state));
            
            StatesSystemV2.UpdateStateTime(ref statesComponent, 0.1f);
            
            Assert.AreEqual(false, statesComponent.HasState(state));
        }
        
        [Test]
        public void Test_TryGetState_NewAPI()
        {
            var states = StatesComponentV2.Create();
            states.Enter(0, 2.0f);
            
            Assert.IsTrue(states.TryGetState(0, out var s0));
            Assert.IsFalse(states.TryGetState(1, out var s1));

            var state = states.GetState(0);

            if (states.TryGetState(0, out var stateTest))
            {
                Assert.AreEqual(state, stateTest);
            }
        }
        
        [Test]
        public void Test_GetState_AsReference()
        {
            var states = StatesComponentV2.Create();
            states.Enter(0, 2.0f);

            var state = states.GetState(0);
            state.updateCount = 5;
            
            Assert.AreEqual(0, states.GetState(0).updateCount);
            
            ref var stateref = ref states.GetState(0);
            stateref.updateCount = 5;
            
            Assert.AreEqual(5, states.GetState(0).updateCount);
        }
    }
}