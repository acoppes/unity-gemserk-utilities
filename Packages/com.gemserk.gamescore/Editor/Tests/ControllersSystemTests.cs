using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Editor.Tests
{
    public class ControllersSystemTests
    {
        private class MockController : MonoBehaviour, IController, IInit, IUpdate
        {
            public int initCalledCount;
            public int updateCalledCount;
            public Action initCallback;
            public Action updateCallback;
            
            public void OnInit(World world, Entity entity)
            {
                initCalledCount++;
                initCallback?.Invoke();
            }

            public void OnUpdate(World world, Entity entity, float dt)
            {
                updateCalledCount++;
                updateCallback?.Invoke();
            }
        }
        
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            gameObject.AddComponent<ControllerInitializationSystem>();
            gameObject.AddComponent<ControllerSystem>();
            world.fixedUpdateParent = world.transform;
            world.Awake();
        }
        
        [TearDown]
        public void AfterEach()
        {
            world.OnDestroy();
            Object.DestroyImmediate(world.gameObject);
            world = null;
        }
        
        [Test]
        public void Test_InitAndUpdate_Controller()
        {
            var controllerPrefab = new GameObject();
            controllerPrefab.AddComponent<MockController>();
            
            var entity = world.CreateEntity(null, null, e =>
            {
                e.Add(new ControllerComponent()
                {
                    prefab = controllerPrefab,
                    sharedInstance = false,
                });
            });
            
            world.FixedUpdate();

            var mockController = entity.Get<ControllerComponent>().controllers[0] as MockController;
            
            Assert.AreEqual(1, mockController.initCalledCount);
            Assert.AreEqual(1, mockController.updateCalledCount);
        }
    }
}