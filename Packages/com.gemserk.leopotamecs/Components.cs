using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public struct NameComponent : IEntityComponent
    {
        public string name;
        public bool singleton;

        public bool cachedInSingletonsDictionary;
    }
    
    public struct LookingDirection : IEntityComponent
    {
        public Vector3 value;
    }
    
    public struct PositionComponent : IEntityComponent
    {
        public Vector3 value;
        public int type;
    }

    public struct PlayerComponent : IEntityComponent
    {
        // will be a flag in the future
        public int player;
        public int playerBitmask => 1 << player;
    }

    public struct DisabledComponent : IEntityComponent
    {
        
    }
    
    public struct EnableDisabledComponent : IEntityComponent
    {
        
    }

    public struct EcsWorldEntitiesDebugComponent : IEntityComponent
    {
        public string name;
        public Vector2 scrollPosition;
        public bool selected;
    }
}