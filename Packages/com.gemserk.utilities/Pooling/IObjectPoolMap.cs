using UnityEngine.Pool;

namespace Gemserk.Utilities.Pooling
{
    public interface IObjectPoolMap<T, K> 
        where T: class where K: class
    {
        T Get(K key);

        PooledObject<T> Get(K key, out T v);

        void Release(T element);

        void Clear();
    }
}