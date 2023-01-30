using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Gemserk.Utilities
{
    public interface IPoolDictionary<T> where T : class
    {
        public IObjectPool<T> GetPool(T prefab);

        void Clear();
    }
    
    public class PoolDictionary : IPoolDictionary<GameObject>
    {
        private readonly Dictionary<GameObject, GameObjectPool> poolDictionary = new();

        private readonly Transform parent;

        // private string poolNameFormat = "[NAME]";

        public PoolDictionary(string poolName)
        {
            var parentGameObject = new GameObject(poolName);
            parent = parentGameObject.transform;
        }
        
        public IObjectPool<GameObject> GetPool(GameObject prefab)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                var poolName = $"{prefab.name}-Pool";
                var gameObjectPool = new GameObjectPool(prefab, poolName);
                gameObjectPool.objectsPoolParent.SetParent(parent);
                
                poolDictionary[prefab] = gameObjectPool;
            }
            return poolDictionary[prefab];
        }

        public void Clear()
        {
            var keys = poolDictionary.Keys;
            foreach (var key in keys)
            {
                poolDictionary[key].Clear();
            }
        }
    }
}