using System.Collections.Generic;
using UnityEngine.Pool;

namespace Game
{
    public static class ObjectPoolExtensions
    {
        public static void PreCreate<T>(this IObjectPool<T> pool, int count, bool startActive = false) where T : class
        {
            var created = new List<T>();
            
            for (int i = 0; i < count; i++)
            {
                created.Add(pool.Get());
            }

            if (!startActive)
            {
                foreach (var instance in created)
                {
                    pool.Release(instance);
                }
            }
        }        
    }
}