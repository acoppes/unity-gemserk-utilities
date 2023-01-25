using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities
{
    public class GameObjectPool : IObjectPool<GameObject>
    {
        private GameObject prefab;
        private Transform objectsPoolParent;

        private IObjectPool<GameObject> delegatePool;
        
        public GameObjectPool(GameObject prefab, string poolName)
        {
            this.prefab = prefab;
            
            var parentGameObject = new GameObject(poolName);
            objectsPoolParent = parentGameObject.transform;

            delegatePool = new LinkedPool<GameObject>(CreateObject, OnGet, OnRelease);
        }

        private void OnRelease(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(objectsPoolParent);
        }

        private GameObject CreateObject()
        {
            return Object.Instantiate(prefab, objectsPoolParent);
        }
        
        private void OnGet(GameObject gameObject)
        {
            gameObject.SetActive(true);
            gameObject.transform.SetParent(null);
        }

        public GameObject Get()
        {
            return delegatePool.Get();
        }

        public PooledObject<GameObject> Get(out GameObject v)
        {
            return delegatePool.Get(out v);
        }

        public void Release(GameObject element)
        {
            delegatePool.Release(element);
        }

        public void Clear()
        {
            delegatePool.Clear();
        }

        public int CountInactive => delegatePool.CountInactive;
    }
}