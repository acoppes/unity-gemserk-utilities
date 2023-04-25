using System.Collections;
using System.Collections.Generic;
using Gemserk.Utilities;
using UnityEngine;

namespace Scenes.ObjectPoolMapExample
{
    public class ObjectPoolMapExample : MonoBehaviour
    {
        public List<GameObject> prefabs = new List<GameObject>();

        private GameObjectPoolMap poolMap = new GameObjectPoolMap();

        private List<GameObject> instances = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnAndDestroy());
        }

        private IEnumerator SpawnAndDestroy()
        {
            yield return null;

            while (true)
            {
                var count = UnityEngine.Random.Range(1, 10);

                for (int i = 0; i < count; i++)
                {
                    var prefab = prefabs.Random();
                    var instance = poolMap.Get(prefab);

                    instance.transform.position = UnityEngine.Random.insideUnitCircle * 7.0f;
                    
                    instances.Add(instance);
                }

                yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));

                foreach (var instance in instances)
                {
                    poolMap.Release(instance);
                }
                
                instances.Clear();
                
                yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));
            }
        }
    }
}
