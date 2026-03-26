using NUnit.Framework;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor.Tests
{
    public class WorldInstancesTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void WorldInstances_Get()
        {
            var gameObject = new GameObject("MyName");
            gameObject.tag = "Player";
            
            var world = gameObject.AddComponent<World>();
            world.Awake();
            Assert.AreEqual(1, WorldInstances.Instances.Count);
            Assert.AreSame(world, WorldInstances.Default);
            Assert.AreSame(world, WorldInstances.GetByName("MyName"));
            Assert.AreSame(world, WorldInstances.GetByTag("Player"));
            world.OnDestroy();
            Assert.AreEqual(0, WorldInstances.Instances.Count);
        }
        
        // [Test]
        // public void WorldInstances_ShouldBeRemoved_OnDestroy()
        // {
        //     var gameObject = new GameObject("MyName");
        //     var world = gameObject.AddComponent<World>();
        //     world.Awake();
        //     world.OnDestroy();
        //     Assert.AreEqual(0, WorldInstances.Instances.Count);
        // }
    }
}