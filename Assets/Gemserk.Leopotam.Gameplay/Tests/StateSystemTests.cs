using Gemserk.Leopotam.Gameplay.Controllers;
using NUnit.Framework;

namespace Gemserk.Leopotam.Gameplay.Tests
{
    public class StateSystemTests
    {
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
    }
}