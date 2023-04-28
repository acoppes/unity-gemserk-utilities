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

#if UNITY_EDITOR
        private Transform objectsPoolParent;
#endif
        
        private IObjectPool<GameObject> delegatePool;

        public GameObjectPool(GameObject prefab, string poolName)
        {
            this.prefab = prefab;
            
            var parentGameObject = new GameObject(poolName);
#if UNITY_EDITOR
            objectsPoolParent = parentGameObject.transform;
#endif

            delegatePool = new LinkedPool<GameObject>(CreateObject, OnGet, OnRelease);
        }

        private void OnRelease(GameObject gameObject)
        {
            gameObject.SetActive(false);
#if UNITY_EDITOR
            gameObject.transform.SetParent(objectsPoolParent);
#endif
        }

        private GameObject CreateObject()
        {
#if UNITY_EDITOR
            var instance = Object.Instantiate(prefab, objectsPoolParent);
#else
            var instance = Object.Instantiate(prefab);
#endif
            var poolInstance = instance.AddComponent<GameObjectPoolInstance>();
            poolInstance.source = prefab;
            // poolInstance.pooled = true;
            return instance;
        }
        
        private void OnGet(GameObject gameObject)
        {
            gameObject.SetActive(true);
            
#if UNITY_EDITOR
            gameObject.transform.SetParent(null);
#endif
            
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