using Game.Components;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class ConfigurationScriptSystemTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            gameObject.AddComponent<ConfigurationScriptSystem>();
            
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
        public void Test_ConfigurationScript_OnCreation()
        {
            var createdEntity = world.CreateEntity(null, null, entity =>
            {
                entity.Add(new MovementComponent()
                {
                    speed = 50f
                });
                entity.Add(new ConfigurationScriptComponent()
                {
                    configurationScript = new ConfigurationScriptFunc((_, e) =>
                    {
                        e.Get<MovementComponent>().speed = 100f;
                    })
                });
            });
            
            Assert.AreEqual(100f, createdEntity.Get<MovementComponent>().speed);
            
            createdEntity.Get<MovementComponent>().speed = 30f;
            
            world.FixedUpdate();
            
            Assert.AreEqual(30f, createdEntity.Get<MovementComponent>().speed);
        }
        
        [Test]
        public void Test_ConfigurationScript_OnReconfigure()
        {
            var createdEntity = world.CreateEntity(null, null, entity =>
            {
                entity.Add(new MovementComponent()
                {
                    speed = 50f
                });
                entity.Add(new ConfigurationScriptComponent()
                {
                    configurationScript = new ConfigurationScriptFunc((_, e) =>
                    {
                        e.Get<MovementComponent>().speed = 100f;
                    })
                });
            });

            createdEntity.Get<MovementComponent>().speed = 30f;
            world.FixedUpdate();
            Assert.AreEqual(30f, createdEntity.Get<MovementComponent>().speed);

            createdEntity.Get<ConfigurationScriptComponent>().reconfigure = true;
            
            world.FixedUpdate();
            Assert.AreEqual(100f, createdEntity.Get<MovementComponent>().speed);
        }
    }
}