using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class MultiSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly List<T> instances = new ();

        public static List<T> Instances => instances;

        public static T Default => instances[0];

        protected virtual void Awake()
        {
            instances.Add(gameObject.GetComponent<T>());
        }

        protected virtual void OnDestroy()
        {
            instances.Remove(gameObject.GetComponent<T>());
        }
    }
}