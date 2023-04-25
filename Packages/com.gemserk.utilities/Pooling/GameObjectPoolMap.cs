using UnityEngine;
using UnityEngine.Pool;

namespace Gemserk.Utilities.Pooling
{
    public class GameObjectPoolMap : ObjectPoolMap<GameObject, GameObject>
    {
        private const string DefaultPrefix = "~Pool";

        private string poolNameFormat;
        private int preCreateCount;

        public GameObjectPoolMap(string poolPrefix = DefaultPrefix, int preCreateCount= 0)
        {
            poolNameFormat = poolPrefix + "-{0}";
            this.preCreateCount = preCreateCount;
        }
        
        protected override IObjectPool<GameObject> GetOrCreatePool(GameObject key)
        {
            if (!pools.ContainsKey(key))
            {
                var pool = new GameObjectPool(key, string.Format(poolNameFormat, key.name));
                pool.PreCreate(preCreateCount);
                pools[key] = pool;
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