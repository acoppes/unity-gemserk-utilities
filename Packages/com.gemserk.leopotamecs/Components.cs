using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public struct NameComponent : IEntityComponent
    {
        public string name;
        public bool singleton;

        public bool cachedInSingletonsDictionary;
    }

    public struct GameObjectComponent : IEntityComponent
    {
        public GameObject gameObject;
    }
}