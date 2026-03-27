using System.Collections.Generic;
using Game.Components;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class OnHealthChangedControllerSystemTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            gameObject.AddComponent<OnHealthChangedControllerSystem>();
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

        private class MockController : IController, IDamagedEvent, IHealedEvent
        {
            public bool damageWasCalled;
            public bool healWasCalled;

            public void Reset()
            {
                damageWasCalled = false;
                healWasCalled = false;
            }
            
            public void OnDamaged(World world, Entity entity)
            {
                damageWasCalled = true;
            }

            public void OnHealed(World world, Entity entity)
            {
                healWasCalled = true;
            }
        }

        [Test]
        public void Controller_Callbacks_WhenDamaged()
        {
            var mockController = new MockController();
                
            var e = world.CreateEntity(null, null, entity =>
            {
                entity.Add(new HealthComponent()
                {
                    damages = new List<HealthChangeData>(),
                    processedDamages = new List<HealthChangeData>(),
                    healEffects = new List<HealthChangeData>(),
                    processedHealEffects = new List<HealthChangeData>(),
                    current = 100,
                    total = 100
                });

                entity.Add(new ControllerComponent()
                {
                    controllers = new List<IController>()
                    {
                        mockController
                    },
                });
            });
            
            e.Get<HealthComponent>().processedDamages.Add(new HealthChangeData()
            {
                value = 20
            });
            
            world.FixedUpdate();
            
            Assert.IsTrue(mockController.damageWasCalled);
            Assert.IsFalse(mockController.healWasCalled);
        }
        
        [Test]
        public void Controller_Callbacks_WhenHealed()
        {
            var mockController = new MockController();
                
            var e = world.CreateEntity(null, null, entity =>
            {
                entity.Add(new HealthComponent()
                {
                    damages = new List<HealthChangeData>(),
                    processedDamages = new List<HealthChangeData>(),
                    healEffects = new List<HealthChangeData>(),
                    processedHealEffects = new List<HealthChangeData>(),
                    current = 100,
                    total = 100
                });

                entity.Add(new ControllerComponent()
                {
                    controllers = new List<IController>()
                    {
                        mockController
                    },
                });
            });

            e.Get<HealthComponent>().processedHealEffects.Add(new HealthChangeData()
            {
                value = 20
            });
            
            world.FixedUpdate();
            
            Assert.IsFalse(mockController.damageWasCalled);
            Assert.IsTrue(mockController.healWasCalled);
        }
    }
}