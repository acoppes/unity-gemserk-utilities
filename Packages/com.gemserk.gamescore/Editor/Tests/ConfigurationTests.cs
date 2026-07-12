using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game.Components;
using Game.Configurations;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
                ["_health.total"] = 300f,
                ["_health.current"] = 50f
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

            config["_health.total"] = 500f;

            createdEntity.Get<ConfigurationComponent>().SetDirty();

            world.FixedUpdate();

            Assert.AreEqual(500f, createdEntity.Get<HealthComponent>().total);
            Assert.AreEqual(50f, createdEntity.Get<HealthComponent>().current);
        }

        [Test]
        public void Test_GetConfiguration_ShouldBeNull()
        {
            {
                var config = new DictionaryConfiguration
                {
                    ["vision"] = 10f
                };

                var healthConfig = config.GetConfiguration("health");
                Assert.IsNull(healthConfig);

                var visionStats = config.GetConfiguration("vision.stats");
                Assert.IsNull(visionStats);
            }

            {
                var config = new JsonConfiguration()
                {
                    ["vision"] = 10f
                };

                var healthConfig = config.GetConfiguration("health");
                Assert.IsNull(healthConfig);

                var visionStats = config.GetConfiguration("vision.stats");
                Assert.IsNull(visionStats);
            }
        }

        [Test]
        public void Test_GetConfiguration_ArrayUsage()
        {
            {
                var config = new DictionaryConfiguration
                {
                    ["listOfValues"] = new DictionaryConfiguration[]
                    {
                        new DictionaryConfiguration
                        {
                            ["key1"] = 100
                        },
                        new DictionaryConfiguration
                        {
                            ["key2"] = 300f
                        }
                    }
                };

                // var listOfConfig = config.GetConfiguration("listOfValues");
                // Assert.IsNotNull(listOfConfig);

                var configurations = config.GetConfigurationArray("listOfValues");
                Assert.IsNotNull(configurations);

                Assert.AreEqual(2, configurations.Length);
                Assert.IsTrue(configurations[0].Has("key1"));
                Assert.IsTrue(configurations[1].Has("key2"));
                Assert.AreEqual(100, configurations[0].Get<int>("key1"));
                Assert.AreEqual(300f, configurations[1].Get<float>("key2"));
            }

            {
                string json = 
                @"
                {
                    ""listOfValues"" : [
                        {
                            ""key1"": 100
                        },
                        {
                            ""key2"" : 300.0
                        }
                    ]
                }
                ";
                
                var jobject = JObject.Parse(json);

                var config = new JsonConfiguration(jobject);

                var configurations = config.GetConfigurationArray("listOfValues");
                Assert.IsNotNull(configurations);

                Assert.AreEqual(2, configurations.Length);
                Assert.IsTrue(configurations[0].Has("key1"));
                Assert.IsTrue(configurations[1].Has("key2"));
                Assert.AreEqual(100, configurations[0].Get<int>("key1"));
                Assert.AreEqual(300f, configurations[1].Get<float>("key2"));
            }
        }
        
        [Test]
        public void Test_ConfigurationSystem_DontExplode()
        {
            var entity1 = world.CreateEntity(null, null, e =>
            {
                e.Add(new ConfigurationComponent
                {
                    configurationKey = "units.unit1",
                    configuration = new JsonConfiguration
                    {
                        ["_health.total"] = "wrong value"
                    }
                });
                e.Add(new HealthComponent());
            });
            
            var entity2 = world.CreateEntity(null, null, e =>
            {
                e.Add(new ConfigurationComponent
                {
                    configurationKey = "units.unit2",
                    configuration = new JsonConfiguration
                    {
                        ["_health.total"] = 300
                    }
                });
                e.Add(new HealthComponent());
            });
            
            world.FixedUpdate();
            
            LogAssert.Expect(LogType.Error, new Regex(".*Failed to configure health for.*"));
            
            Assert.AreEqual(0, entity1.Get<HealthComponent>().total);
            Assert.AreEqual(300, entity2.Get<HealthComponent>().total);
        }
    }
}