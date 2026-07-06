using System.Collections.Generic;
using Game.Components;
using Game.Configurations;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class ConfigurationTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            gameObject.AddComponent<ConfigurationSystem>();
            
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
        public void Test_BasicConfiguration()
        {
            var configuration = new DictionaryConfiguration();
            configuration["health"] = new DictionaryConfiguration();
            configuration["health.total"] = 350f;
            
            Assert.IsNotNull(configuration.Get<object>("health"));
            Assert.AreEqual(350, configuration.Get<float>("health.total"));
        }
        
        [Test]
        public void Test_ConfigurationJson()
        {
            var configuration = new JsonConfiguration();
            configuration["health"] = new JsonConfiguration();
            configuration["health.total"] = 350f;
            
            Assert.IsNotNull(configuration.Get<object>("health"));
            Assert.AreEqual(350, configuration.Get<float>("health.total"));
        }
        
        [Test]
        public void Test_Configuration_CreateSubConfigurations()
        {
            var configuration = new JsonConfiguration();
            configuration["health.total"] = 350f;
            
            var dictionaryConfiguration = new DictionaryConfiguration();
            dictionaryConfiguration["health.total"] = 250f;
            
            Assert.AreEqual(350, configuration.Get<float>("health.total"));
            Assert.AreEqual(250, dictionaryConfiguration.Get<float>("health.total"));
        }
        
        [Test]
        public void Test_ConfigurationScript_OnCreation()
        {
            var config = new DictionaryConfiguration
            {
                ["health.total"] = 300f,
                ["health.current"] = 50f
            };
            
            var createdEntity = world.CreateEntity(null, null, entity =>
            {
                entity.Add(new HealthComponent()
                {
                    total = 100f
                });
                
                entity.Add(new ConfigurationComponent()
                {
                    configuration = config
                });
            });
            
            world.FixedUpdate();
                        
            Assert.AreEqual(300f, createdEntity.Get<HealthComponent>().total);
            Assert.AreEqual(50f, createdEntity.Get<HealthComponent>().current);

            config["health.total"] = 500f;
            
            createdEntity.Add(new ConfigurationReconfigureComponent());
            
            world.FixedUpdate();
            
            Assert.AreEqual(500f, createdEntity.Get<HealthComponent>().total);
            Assert.AreEqual(50f, createdEntity.Get<HealthComponent>().current);
        }
        
    }
}