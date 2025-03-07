using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class EntityTests
    {
        [Test]
        public void Test_EntityConstructor()
        {
            var newEntity = new Entity();
            var createdEntity = Entity.Create(null, 0, 0);
            
            Assert.AreEqual(Entity.NullEntity, createdEntity);
            Assert.AreEqual(Entity.NullEntity, newEntity);
            
            Assert.IsTrue(createdEntity == Entity.NullEntity);
            Assert.IsFalse(createdEntity != Entity.NullEntity);
            Assert.IsTrue(createdEntity.Equals(Entity.NullEntity));
            
            Assert.IsTrue(newEntity == Entity.NullEntity);
            Assert.IsFalse(newEntity != Entity.NullEntity);
            Assert.IsTrue(newEntity.Equals(Entity.NullEntity));
        }
        
        [Test]
        public void CreatedEntity_IsNotNullEntity()
        {
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            world.Init();

            var entity = world.CreateEntity();
            Assert.AreEqual(0, entity.ecsEntity);
            Assert.AreEqual(1, entity.ecsGeneration);

            Assert.IsTrue(entity != Entity.NullEntity);
            Assert.IsFalse(entity == Entity.NullEntity);
            
            Assert.IsFalse(entity.Equals(Entity.NullEntity));
        }
        
        [Test]
        public void EntityFromWorld_CantBeNullEntity()
        {
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            world.Init();

            var entity = world.CreateEntity();
            Assert.AreEqual(0, entity.ecsEntity);
            Assert.AreEqual(1, entity.ecsGeneration);

            entity.ecsGeneration = 0;

            Assert.IsTrue(entity != Entity.NullEntity);
            Assert.IsFalse(entity == Entity.NullEntity);
            
            Assert.IsFalse(entity.Equals(Entity.NullEntity));
        }
        
        [Test]
        public void DestroyedEntity_ShouldNotExist()
        {
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            world.Init();

            var entity = world.CreateEntity();
            world.DestroyEntity(entity);
            
            Assert.IsFalse(entity.Exists());
        }
    }
}