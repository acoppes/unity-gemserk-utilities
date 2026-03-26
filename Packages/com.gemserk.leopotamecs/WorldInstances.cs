using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Gemserk.Leopotam.Ecs
{
    public static class WorldInstances
    {
        private static readonly List<World> instances = new ();

        public static List<World> Instances => instances;

        public static World Default => instances.Count > 0 ? instances[0] : null;

        // [RuntimeInitializeOnLoadMethod]
        // static void Initialize()
        // {
        //     // try to autoremove broken instances on initialization
        //     instances?.RemoveAll(i => !i || !i.gameObject);
        // }

        public static World GetByName(string name)
        {
            return instances.First(i => i.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        
        public static World GetByTag(string tag)
        {
            return instances.First(i => i.tag.Equals(tag, StringComparison.OrdinalIgnoreCase));
        }
        
        public static World GetByScene(Scene scene)
        {
            return instances.First(i => i.gameObject.scene == scene);
        }
        
        public static World GetBySceneName(string name)
        {
            return instances.First(i => i.gameObject.scene.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static void Register(World world)
        {
            instances.Add(world);
        }

        public static void Unregister(World world)
        {
            instances.Remove(world);
        }
    }
}