using System.Collections.Generic;
using UnityEngine.Pool;

namespace Gemserk.Utilities.Pooling
{
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
}