using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Pooling
{
    public class GameObjectPool : IObjectPool<GameObject>
    {
        public class GameObjectPoolInstance : PoolInstance<GameObject>
        {
            
        }

        private GameObject prefab;

        public Transform objectsPoolParent;

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
            var instance = Object.Instantiate(prefab, objectsPoolParent);
            var poolInstance = instance.AddComponent<GameObjectPoolInstance>();
            poolInstance.source = prefab;
            // poolInstance.pooled = true;
            return instance;
        }
        
        private void OnGet(GameObject gameObject)
        {
            gameObject.SetActive(true);
            gameObject.transform.SetParent(null);
            
            // var poolInstance = gameObject.GetComponent<GameObjectPoolInstance>();
            // poolInstance.pooled = false;
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
            
            // var poolInstance = element.GetComponent<GameObjectPoolInstance>();
            // poolInstance.pooled = true;
        }

        public void Clear()
        {
            delegatePool.Clear();
        }

        public int CountInactive => delegatePool.CountInactive;
    }
}