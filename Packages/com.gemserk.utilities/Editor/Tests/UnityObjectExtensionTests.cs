using Gemserk.Utilities.Pooling;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Gemserk.Utilities.Tests
{
    public class UnityObjectExtensionTests
    {
        private class TestComponent : Component
        {
            
        }
        
        [Test]
        public void Test_ObjectExtensions_GetInterface_WhenNullObjects()
        {
            Object nullObject = null;
            GameObject nullGameObject = null;
            Component nullComponent = null;

            Assert.IsNull(nullObject.GetInterface<TestComponent>());
            Assert.IsNull(nullGameObject.GetInterface<TestComponent>());
            Assert.IsNull(nullComponent.GetInterface<TestComponent>());
        }
        
        [Test]
        public void Test_ObjectExtensions_GetInterface_WhenDestroyed()
        {
            GameObject destroyedGameObject = new GameObject();
            Object.DestroyImmediate(destroyedGameObject);
            
            Assert.IsNull(destroyedGameObject.GetInterface<TestComponent>());
        }
    }
}