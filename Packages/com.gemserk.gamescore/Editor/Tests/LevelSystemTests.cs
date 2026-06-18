using Game.Components;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class LevelSystemTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            gameObject.AddComponent<LevelUpSystem>();
            gameObject.AddComponent<OnLevelUpEventControllerSystem>();
            
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
        public void PendingLevelUpEvent_AfterLevelUp()
        {
            var entity = world.CreateEntity(null, null, e =>
            {
                e.Add(new LevelComponent()
                {
                    current = 0,
                    max = 10,
                    next = 0,
                    previous = 0
                });
            });

            entity.Get<LevelComponent>().next = 1;
            world.FixedUpdate();
            
            Assert.IsTrue(entity.Has<LevelChangedEventComponent>());
            world.FixedUpdate();
            
            Assert.IsFalse(entity.Has<LevelChangedEventComponent>());
        }
    }
}