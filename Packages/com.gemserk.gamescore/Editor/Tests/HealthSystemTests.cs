using System.Collections.Generic;
using Game.Components;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class HealthSystemTests
    {
        private World world;
        private HealthSystem healthSystem;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            gameObject.AddComponent<HealthSystem>();
            world.fixedUpdateParent = world.transform;
            world.Awake();
        }
        
        [TearDown]
        public void AfterEach()
        {
            world.OnDestroy();
            Object.DestroyImmediate(world.gameObject);
            world = null;
            healthSystem = null;
        }
        
        [Test]
        public void Health_ProcessedDamage_Positive()
        {
            var health = new HealthComponent
            {
                total = 100,
                current = 50
            };

            var result = HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 15
            });
            
            Assert.AreEqual(15, result.value, 0.1f);
            Assert.AreEqual(35, health.current, 0.1f);
        }
        
        [Test]
        public void Health_ProcessedDamage_WhenNegative()
        {
            var health = new HealthComponent
            {
                total = 100,
                current = 25
            };

            var result = HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 100
            });
            
            Assert.AreEqual(25, result.value, 0.1f);
            Assert.AreEqual(0, health.current, 0.1f);
            
            var result2 = HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 25
            });
            
            Assert.AreEqual(0, result2.value, 0.1f);
            Assert.AreEqual(0, health.current, 0.1f);
        }
        
        [Test]
        public void NormalDamage_WhenNoDamageResistance()
        {
            var health = new HealthComponent
            {
                total = 50,
                current = 50
            };

            HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 15
            });
            
            Assert.AreEqual(35, health.current, 0.1f);
        }
        
        [Test]
        public void ReducedDamageHalf_WhenDamageResistance()
        {
            var health = new HealthComponent
            {
                total = 50,
                current = 50,
                damageResistance = 0.5f
            };

            HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 20
            });
            
            Assert.AreEqual(40, health.current, 0.1f);

            health.damageResistance = 1f;
            HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 20
            });
            
            Assert.AreEqual(40, health.current, 0.1f);
        }
        
        [Test]
        public void DamageResult_AffectedByDamageResistance()
        {
            var health = new HealthComponent
            {
                total = 50,
                current = 50,
                damageResistance = 0.5f
            };

            var damageResult = HealthSystem.ProcessDamage(ref health, new HealthChangeData()
            {
                value = 20
            });
            
            Assert.AreEqual(10, damageResult.value, 0.1f);
        }
        
        [Test]
        public void HealthSystem_ProcessHealAndDamage()
        {
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
            });
            
            e.Get<HealthComponent>().damages.Add(new HealthChangeData()
            {
                value = 20
            });
            
            world.FixedUpdate();
            
            Assert.AreEqual(80, e.Get<HealthComponent>().current, 0.01f);
            Assert.AreEqual(1, e.Get<HealthComponent>().processedDamages.Count);
            Assert.AreEqual(0, e.Get<HealthComponent>().processedHealEffects.Count);
            
            e.Get<HealthComponent>().healEffects.Add(new HealthChangeData()
            {
                value = 10
            });
            
            world.FixedUpdate();
            
            Assert.AreEqual(90, e.Get<HealthComponent>().current, 0.01f);
            Assert.AreEqual(0, e.Get<HealthComponent>().processedDamages.Count);
            Assert.AreEqual(1, e.Get<HealthComponent>().processedHealEffects.Count);
            
            world.FixedUpdate();
            
            Assert.AreEqual(90, e.Get<HealthComponent>().current, 0.01f);
            Assert.AreEqual(0, e.Get<HealthComponent>().processedDamages.Count);
            Assert.AreEqual(0, e.Get<HealthComponent>().processedHealEffects.Count);
        }
    }
}