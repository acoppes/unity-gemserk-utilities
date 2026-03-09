using System.Collections;
using Gemserk.Utilities.Pooling;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Gemserk.Utilities.Tests
{
    public class GameObjectPoolTests
    {
        [Test]
        public void PooledInstances_ShouldHave_PoolInstanceComponent()
        {
            var prefab = new GameObject("SuperPrefab");

            var pool = new GameObjectPool(prefab, "MySuperPoolName");

            var instance = pool.Get();
            var poolInstance = instance.GetComponent<GameObjectPool.GameObjectPoolInstance>();
            
            Assert.IsNotNull(poolInstance);
            Assert.AreEqual(prefab, poolInstance.source);
        }
        
        [UnityTest]
        public IEnumerator TestGameObjectPool()
        {
            var prefab = new GameObject("SuperPrefab");

            var pool = new GameObjectPool(prefab, "MySuperPoolName");

            var instance = pool.Get();
            Assert.IsTrue(instance.activeSelf);
            
            Assert.AreEqual(0, pool.CountInactive);
            
            pool.Release(instance);
            Assert.IsFalse(instance.activeSelf);
            
            Assert.AreEqual(1, pool.CountInactive);
        
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestGameObjectMapPool()
        {
            var prefab1 = new GameObject("SuperPrefab1");
            var prefab2 = new GameObject("SuperPrefab2");

            var poolMap = new GameObjectPoolMap();
            var instance1 = poolMap.Get(prefab1);
            var instance2 = poolMap.Get(prefab2);

            yield return null;
            
            poolMap.Release(instance1);
            poolMap.Release(instance2);

            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);

            Assert.IsFalse(instance1.activeSelf);
            Assert.IsFalse(instance2.activeSelf);

            yield return null;
        }
    }
}
