using Game.Components;
using Game.Systems;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class AutoDestroyEntitiesMaxByTypeSystemTests
    {
        private AutoDestroyEntitiesMaxByTypeSystem autoDestroyEntitiesMaxByTypeSystem;
        private EcsWorld ecsWorld;
        private EcsSystems ecsSystems;

        private class MockTypeMax : ITypeMax
        {
            public int max;
            
            public int GetMax()
            {
                return max;
            }
        }
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject();
            autoDestroyEntitiesMaxByTypeSystem = gameObject.AddComponent<AutoDestroyEntitiesMaxByTypeSystem>();

            ecsWorld = new EcsWorld();
            ecsSystems = new EcsSystems(ecsWorld);

            ecsSystems.Add(autoDestroyEntitiesMaxByTypeSystem);
            
            ecsSystems.Init();
            ecsSystems.Inject();
        }

        private int CreateTestEntity(ITypeMax typeMax, int aliveUpdates = 0)
        {
            var e = ecsWorld.NewEntity();
            ref var maxByType = ref ecsWorld.GetPool<MaxByTypeComponent>().Add(e);
            
            maxByType.type = typeMax;
            maxByType.updates = aliveUpdates;
            
            ecsWorld.GetPool<DestroyableComponent>().Add(e);
            return e;
        }
        
        [Test]
        public void ShouldDestroy_IfMarkedForDestroy_MaxReached()
        {
            var e = CreateTestEntity(new MockTypeMax() { max = 0 }, 1);
            ecsSystems.Run();
            Assert.IsTrue(ecsWorld.GetPool<DestroyableComponent>().Get(e).destroy);
        }
        
        [Test]
        public void ShouldNotDestroy_IfMaxNotReached()
        {
            var e = ecsWorld.NewEntity();
            ref var maxByTypeComponent = ref ecsWorld.GetPool<MaxByTypeComponent>().Add(e);
            maxByTypeComponent.type = new MockTypeMax() { max = 5 };
            
            ref var destroyable = ref ecsWorld.GetPool<DestroyableComponent>().Add(e);
            
            ecsSystems.Run();
            
            Assert.IsFalse(destroyable.destroy);
        }

        [Test]
        public void ShouldDestroySome_IfMaxReached_SameType()
        {
            var max = new MockTypeMax() { max = 1 };
            var e1 = CreateTestEntity(max);
            var e2 = CreateTestEntity(max);
            
            ecsSystems.Run();
            
            Assert.IsTrue(ecsWorld.GetPool<DestroyableComponent>().Get(e1).destroy);
            Assert.IsFalse(ecsWorld.GetPool<DestroyableComponent>().Get(e2).destroy);
        }
        
        [Test]
        public void ShouldDestroySome_IfMaxReached_SameType_FirstOldEntities()
        {
            var max = new MockTypeMax() { max = 1 };
            var e1 = CreateTestEntity(max, 0);
            var e2 = CreateTestEntity(max, 1);
            
            ecsSystems.Run();
            
            Assert.IsFalse(ecsWorld.GetPool<DestroyableComponent>().Get(e1).destroy);
            Assert.IsTrue(ecsWorld.GetPool<DestroyableComponent>().Get(e2).destroy);
        }
        
        [Test]
        public void ShouldDestroySome_IfMaxReached_SameType_FirstOldEntities2()
        {
            var max = new MockTypeMax() { max = 1 };
            var e1 = CreateTestEntity(max, 3);
            var e2 = CreateTestEntity(max, 5);
            
            ecsSystems.Run();
            
            Assert.IsFalse(ecsWorld.GetPool<DestroyableComponent>().Get(e1).destroy);
            Assert.IsTrue(ecsWorld.GetPool<DestroyableComponent>().Get(e2).destroy);
        }
        
        [Test]
        public void ShouldNot_ReachMax_IfDifferentType()
        {
            var e1 = CreateTestEntity(new MockTypeMax() { max = 1 });
            var e2 = CreateTestEntity(new MockTypeMax() { max = 1 });
            var e3 = CreateTestEntity(new MockTypeMax() { max = 1 });
            
            ecsSystems.Run();
            
            Assert.IsFalse(ecsWorld.GetPool<DestroyableComponent>().Get(e1).destroy);
            Assert.IsFalse(ecsWorld.GetPool<DestroyableComponent>().Get(e2).destroy);
            Assert.IsFalse(ecsWorld.GetPool<DestroyableComponent>().Get(e3).destroy);
        }
    }
}