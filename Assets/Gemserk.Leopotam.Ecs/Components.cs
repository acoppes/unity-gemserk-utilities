using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{ 
    public struct PositionComponent : IEntityComponent
    {
        public Vector2 value;
    }
    
    public struct NameComponent : IEntityComponent
    {
        public string name;
        public bool singleton;
    }
    
    public struct PlayerComponent : IEntityComponent
    {
        public int player;
    }

    public struct Target
    {
        public int entity;
        public int player;
        public Vector2 position;
        
        public object extra;
    }

    // public struct TargetPosition
    // {
    //     public Target target;
    // }

    public struct TargetComponent : IEntityComponent
    {
        public Target target;
    }
}