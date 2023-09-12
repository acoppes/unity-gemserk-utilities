using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemserk.Leopotam.Ecs
{
    public class MultiSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly List<T> instances = new ();

        public static List<T> Instances => instances;

        public static T Default => instances[0];

        public static T GetByName(string name)
        {
            return instances.First(i => i.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        
        public static T GetByTag(string tag)
        {
            return instances.First(i => i.tag.Equals(tag, StringComparison.OrdinalIgnoreCase));
        }
        
        public static T GetByScene(Scene scene)
        {
            return instances.First(i => i.gameObject.scene == scene);
        }
        
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