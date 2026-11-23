using System;
using Game.Components;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Gemserk.Utilities.Signals;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Editor.Tests
{
    public class TimerSystemTests
    {
        private World world;
        private TimerSystem timerSystem;
        private SignalAsset onTimerCompleteSignalAsset;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            timerSystem = gameObject.AddComponent<TimerSystem>();
            onTimerCompleteSignalAsset = ScriptableObject.CreateInstance<SignalAsset>();
            timerSystem.onTimerCompletedSignal = onTimerCompleteSignalAsset;
            
            world.fixedUpdateParent = world.transform;
            
            world.Awake();
        }
        
        [TearDown]
        public void AfterEach()
        {
            world.OnDestroy();
            Object.DestroyImmediate(timerSystem);
            Object.DestroyImmediate(world.gameObject);
            Object.DestroyImmediate(onTimerCompleteSignalAsset);
            world = null;
        }
        
        [Test]
        public void TestSignalOnFirstUpdate()
        {
            var entity = world.CreateEntity(null, null, e =>
            {
                e.Add(new TimerComponent()
                {
                    timer = new Cooldown(1),
                    paused = false,
                    wasReady = false
                });
            });

            var signalCalled = false;
            
            onTimerCompleteSignalAsset.Register(data =>
            {
                var signalEntity = (Entity)data;
                if (signalEntity.Get<TimerComponent>().wasReady)
                {
                    throw new Exception("shouldnt be ready");
                }
                signalCalled = true;
            });

            entity.Get<TimerComponent>().timer.Fill();
            
            world.FixedUpdate();
            
            Assert.IsTrue(signalCalled);
        }
    }
}