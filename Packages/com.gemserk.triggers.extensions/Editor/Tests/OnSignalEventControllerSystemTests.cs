using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Triggers.Systems;
using Gemserk.Utilities.Signals;
using NUnit.Framework;
using UnityEngine;

namespace Gemserk.Triggers.Tests
{
    public class OnSignalEventControllerSystemTests
    {
        private World world;
        private SignalAsset testSignalAsset;

        private class TestControllerForSignal : ControllerBase, ISignalControllerEvent
        {
            public int calls;
            public ISignal calledSignal;
            
            public void OnGlobalSignal(World world, Entity entity, ISignal sourceSignal, object userData)
            {
                calls++;
                calledSignal = sourceSignal;
            }
        }

        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject();
            world = gameObject.AddComponent<World>();
            testSignalAsset = ScriptableObject.CreateInstance<SignalAsset>();
            
            gameObject.AddComponent<ControllerInitializationSystem>();
            gameObject.AddComponent<ControllerSystem>();
            
            var signalsEventSystem = gameObject.AddComponent<OnSignalEventControllerSystem>();
            signalsEventSystem.signalAssets.Add(testSignalAsset);
            
            world.fixedUpdateParent = world.transform;
            
            world.Awake();
        }

        [TearDown]
        public void AfterEach()
        {
            Object.DestroyImmediate(testSignalAsset);
        }
        
        private Entity CreateEntityWithController()
        {
            var controllerPrefab = new GameObject();
            controllerPrefab.AddComponent<TestControllerForSignal>();
            
            var e = world.CreateEntityCustom(e =>
            {
                e.Add(new ControllerComponent()
                {
                    prefab = controllerPrefab
                });
            });
            
            return e;
        }
        
        [Test]
        public void Controller_ShouldNotBeCalled_WhenSignalNotInvoked()
        {
            var entity = CreateEntityWithController();
            
            world.FixedUpdate();

            var testController = entity.Get<ControllerComponent>().controllers[0] as TestControllerForSignal;
            
            Assert.AreEqual(0, testController.calls);
            Assert.IsNull(testController.calledSignal);
        }
        
        [Test]
        public void Controller_ShouldBeCalled_WhenSignalInvoked()
        {
            var entity = CreateEntityWithController();
            
            world.FixedUpdate();
            
            testSignalAsset.Signal(null);

            var testController = entity.Get<ControllerComponent>().controllers[0] as TestControllerForSignal;
            
            Assert.AreEqual(1, testController.calls);
            Assert.AreSame(testSignalAsset, testController.calledSignal);
        }
    }
}
