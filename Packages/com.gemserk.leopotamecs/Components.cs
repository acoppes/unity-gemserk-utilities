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
    
    public struct LookingDirection : IEntityComponent
    {
        public Vector3 value;
    }
    
    public struct PositionComponent : IEntityComponent
    {
        public Vector3 value;
    }

    public struct PlayerComponent : IEntityComponent
    {
        public int player;
    }

    public struct DisabledComponent : IEntityComponent
    {
        
    }
}