using System;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class EntityTests
    {
        private struct ComponentA : IEntityComponent
        {
            
        }
        
        private struct ComponentB : IEntityComponent
        {
            
        }
        
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

            var e = world.CreateEntity();
            world.DestroyEntity(e);
            
            Assert.IsFalse(e.Exists());
        }
        
        [Test]
        public void NewEntityFromConstructor_ShouldNotExist()
        {
            var e = new Entity();
            Assert.IsFalse(e.Exists());
            
            Assert.IsFalse(Entity.NullEntity.Exists());
        }
        
        [Test]
        public void CreatedEntity_ShouldExist()
        {
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            world.Init();
            
            var e = world.CreateEntity();
            Assert.IsTrue(e.Exists());
        }
        
        [Test]
        public void EntityRemovedFromFilter_ShouldExist()
        {
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            world.Init();
            
            var e = world.CreateEntity();
            world.AddComponent(e, new ComponentA());
            world.AddComponent(e, new ComponentB());
            Assert.IsTrue(e.Exists());

            var componentsA = world.EcsWorld.GetPool<ComponentA>();
            // var componentsB = world.EcsWorld.GetPool<ComponentB>();
            
            componentsA.Del(e);
            
            Assert.IsTrue(e.Exists());
            Assert.IsFalse(world.HasComponent<ComponentA>(e));

            world.EcsWorld.DelEntity(e);
            
            Assert.IsFalse(e.Exists());
            
            // Assert.IsFalse(componentsB.Has(e));
            // Assert.IsFalse(world.HasComponent<ComponentB>(e));
        }
        
        [Test]
        public void World_Exception_OnDestroyedEntity()
        {
            var gameObject = new GameObject();
            var world = gameObject.AddComponent<World>();
            world.Init();
            
            var e = world.CreateEntity();
            world.AddComponent(e, new ComponentA());
            Assert.IsTrue(e.Exists());
            var componentsA = world.EcsWorld.GetPool<ComponentA>();
            world.EcsWorld.DelEntity(e);
            Assert.Throws<Exception>(() => componentsA.Has(e));
        }
    }
}