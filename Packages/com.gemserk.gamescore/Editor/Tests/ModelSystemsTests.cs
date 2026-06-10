using Game.Components;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class ModelSystemsTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            gameObject.AddComponent<ModelCreationSystem>();
            gameObject.AddComponent<ModelUpdateSystem>();
            
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
        public void Model_WithoutPrefab_ShouldFailUpdate()
        {
            world.CreateEntity(null, null, entity =>
            {
                entity.Add(new ModelComponent()
                {
                    prefab = null
                });
            });
            
            world.FixedUpdate();
        }
    }
}