using NUnit.Framework;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Tests
{
    public class EntityGenerationTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestEntityValid()
        {
            // Use the Assert class to test conditions
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            
            world.Init();

            var entity = world.CreateEntity();
            
            Assert.IsTrue(world.Exists(entity));
            
            world.DestroyEntity(entity);
            
            Assert.IsFalse(world.Exists(entity));
            
            var entity2 = world.CreateEntity();
            
            Assert.AreNotEqual(entity, entity2);
            
            Debug.Log(entity);
            Debug.Log(entity2);
        }
    }
}
