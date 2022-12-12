using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Tests
{
    public class WorldApiTests
    {
        public struct TestComponent
        {
            public int value;
        }
        
        [Test]
        public void ComponentShouldBeModified_After_TryGetComponent()
        {
            // Use the Assert class to test conditions
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            
            world.Init();

            var entity = world.CreateEntity();

            TestComponent testComponent;
            
            if (world.TryGetComponent(entity, out testComponent))
            {
                Assert.Fail("Shouldn't have this component");
            }
            
            world.AddComponent(entity, new TestComponent()
            {
                value = 54
            });
            
            if (world.TryGetComponent(entity, out testComponent))
            {
                Assert.AreEqual(54, testComponent.value);
            }

            var otherTestComponent = world.GetComponent<TestComponent>(entity);
            Assert.AreEqual(54, otherTestComponent.value);
        }
    }
}