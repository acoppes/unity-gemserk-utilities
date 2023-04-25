using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities
{
    public interface IObjectPoolMap<T, K> 
        where T: class where K: class
    {
        T Get(K key);

        PooledObject<T> Get(K key, out T v);

        void Release(T element);

        void Clear();
    }

    public abstract class ObjectPoolMap<T, K> : IObjectPoolMap<T, K> 
        where T : class 
        where K : class
    {
        protected IDictionary<K, IObjectPool<T>> pools = new Dictionary<K, IObjectPool<T>>();

        protected abstract IObjectPool<T> GetOrCreatePool(K key);
        
        public T Get(K key)
        {
            return GetOrCreatePool(key).Get();
        }

        public PooledObject<T> Get(K key, out T v)
        {
            return GetOrCreatePool(key).Get(out v);
        }

        public abstract void Release(T element);

        public void Clear()
        {
            foreach (var key in pools.Keys)
            {
                pools[key].Clear();
            }
            pools.Clear();
        }
    }

    public class GameObjectPoolMap : ObjectPoolMap<GameObject, GameObject>
    {
        private const string PoolNameFormat = "~Pool-{0}";
        
        protected override IObjectPool<GameObject> GetOrCreatePool(GameObject key)
        {
            if (!pools.ContainsKey(key))
            {
                pools[key] = new GameObjectPool(key, string.Format(PoolNameFormat, key.name));
            }
            return pools[key];
        }

        public override void Release(GameObject element)
        {
            var poolInstance = element.GetComponent<GameObjectPool.GameObjectPoolInstance>();
            if (poolInstance != null && poolInstance.source != null)
            {
                var pool = GetOrCreatePool(poolInstance.source);
                pool.Release(element);
            }
            else
            {
                Object.Destroy(element);
            }
        }
    }
}