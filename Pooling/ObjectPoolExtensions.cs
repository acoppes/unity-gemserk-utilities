using System.Collections.Generic;
using UnityEngine.Pool;

namespace Gemserk.Utilities.Pooling
{
    public static class ObjectPoolExtensions
    {
        public static void PreCreate<T>(this IObjectPool<T> pool, int count) where T : class
        {
            var instances = new List<T>();
            
            for (var i = 0; i < count; i++)
            {
                var instance = pool.Get();
                instances.Add(instance);
            }

            foreach (var instance in instances)
            {
                pool.Release(instance);
            }
        }    
    }
}